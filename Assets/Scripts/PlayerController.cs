using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float AnimationDampTime = 0.1f;
    public float CameraRayLength = 100f;
    public float DashDuration = 0.3f;
    public float DashSpeed = 10f;
    public float GroundedRayLength = 1.2f;
    public float InputThreshold = 0.1f;
    public float JumpDuration = 0.2f;
    public float JumpSpeed = 8f;
    public float KnockbackDuration = 0.3f;
    public float KnockbackSpeed = 8f;
    public float MoveSpeed = 5f;
    public int   ShootDamage = 1;
    public float ShootEffectTime = 0.2f;
    public float ShootFireRate = 0.15f;
    public float ShootRange = 100f;
    public float WallJumpLockDuration = 0.15f;
    public Vector3 WallJumpBoxHalfSize = new Vector3(0.5f, 0.5f, 0.35f);

    private float mDashTimer;
    private bool  mDashTriggered;
    private float mInputDash;
    private float mInputFire;
    private float mInputHorizontal;
    private float mInputJump;
    private bool  mIsGrounded;
    private float mJumpTimer;
    private bool  mJumpTriggered;
    private float mKnockbackTimer;
    private float mShootTimer;
    private float mShootEffectTimer;
    private float mWallJumpLockTimer;

    private Vector3 mAimDirection;
    private Vector3 mLookDirection;
    private Ray mShootRay;

    private Animator mAnimator;
    private LayerMask mLayerMaskAimPlane;
    private LayerMask mLayerMaskDestructible;
    private LayerMask mLayerMaskShootable;
    private LayerMask mLayerMaskEnvironment;
    private Rigidbody mRigidbody;

    private AudioSource mWeaponEffectAudio;
    private Light mWeaponEffectLight;
    private LineRenderer mWeaponEffectLine;
    private ParticleSystem mWeaponEffectParticles;
    private Transform mWeapon;

    private void Start()
    {
        mDashTimer = 0f;
        mDashTriggered = false;
        mIsGrounded = false;
        mJumpTimer = 0f;
        mJumpTriggered = false;
        mKnockbackTimer = 0f;
        mShootRay = new Ray();
        mShootTimer = 0f;
        mShootEffectTimer = 0f;
        mWallJumpLockTimer = 0f;

        mAimDirection = Vector3.right;
        mLookDirection = Vector3.right;

        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();

        int aimPlane = LayerMask.GetMask("AimPlane");
        int destructible = LayerMask.GetMask("Destructible");
        int environment = LayerMask.GetMask("Environment");
        mLayerMaskAimPlane = aimPlane;
        mLayerMaskDestructible = destructible;
        mLayerMaskEnvironment = environment;
        mLayerMaskShootable = destructible | environment;

        mWeapon = transform.Find("WeaponEffect");
        GameObject weaponEffect = mWeapon.gameObject;
        mWeaponEffectAudio = weaponEffect.GetComponent<AudioSource>();
        mWeaponEffectLight = weaponEffect.GetComponent<Light>();
        mWeaponEffectLine = weaponEffect.GetComponent<LineRenderer>();
        mWeaponEffectParticles = weaponEffect.GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (mKnockbackTimer < Time.time)
        {
            InputHandler();
            IsGrounded();
            DirectionHandler();
            MovementHandler();
            RotationHandler();
            ShootHandler();
            AnimationHandler();
        }
    }

    public void Knockback(Vector3 hitpoint)
    {
        mKnockbackTimer = Time.time + KnockbackDuration;

        Vector3 direction = mRigidbody.position + mRigidbody.centerOfMass - hitpoint;
        mRigidbody.velocity = direction.normalized * KnockbackSpeed;
    }

    private void AnimationHandler()
    {
        float movement = Mathf.Sign(mLookDirection.x) * mInputHorizontal;
        mAnimator.SetFloat("Movement", movement, AnimationDampTime, Time.deltaTime);
    }

    private bool CanWallJump()
    {
        // Check player horizontal input
        if (mInputHorizontal < -InputThreshold || mInputHorizontal > InputThreshold)
        {
            // Check if the player is near a wall
            float horizontal = Mathf.Sign(mInputHorizontal);
            Vector3 offset = new Vector3(horizontal * WallJumpBoxHalfSize.x, 0f, 0f);
            Vector3 center = mRigidbody.position + mRigidbody.centerOfMass + offset;

            return Physics.CheckBox(center, WallJumpBoxHalfSize, Quaternion.identity, mLayerMaskEnvironment, QueryTriggerInteraction.Ignore);
        }
        return false;
    }

    private void DirectionHandler()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(cameraRay, out hitInfo, CameraRayLength, mLayerMaskDestructible, QueryTriggerInteraction.Ignore))
        {
            // Raycast hit destructible object. Check if EnemyHealth component exist
            EnemyHealth enemyHealth = hitInfo.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Aim and look towards enemy center
                Rigidbody enemyRigidbody = hitInfo.collider.GetComponent<Rigidbody>();
                Vector3 aimDirection = enemyRigidbody.position + enemyRigidbody.centerOfMass - mWeapon.position;
                Vector3 lookDirection = enemyRigidbody.position + enemyRigidbody.centerOfMass - mRigidbody.position + mRigidbody.centerOfMass;

                // Normalize direction vectors
                mAimDirection = aimDirection.normalized;
                mLookDirection = lookDirection.normalized;

            }
        }
        else if (Physics.Raycast(cameraRay, out hitInfo, CameraRayLength, mLayerMaskAimPlane))
        {
            // Raycast hit AimPlane. Aim and look left or right
            Vector3 aimDirection = hitInfo.point - mWeapon.position;
            Vector3 lookDirection = hitInfo.point - mRigidbody.position + mRigidbody.centerOfMass;

            // Set Z value to zero
            aimDirection.z = 0f;
            lookDirection.z = 0f;

            // Normalize direction vectors
            mAimDirection = aimDirection.normalized;
            mLookDirection = lookDirection.normalized;
        }
    }

    private void InputHandler()
    {
        mInputDash = Input.GetAxis("Dash");
        mInputFire = Input.GetAxis("Fire1");
        mInputHorizontal = Input.GetAxis("Horizontal");
        mInputJump = Input.GetAxis("Jump");
    }

    private void IsGrounded()
    {
        Vector3 center = mRigidbody.position + mRigidbody.centerOfMass;
        Vector3 down = Vector3.down;
        mIsGrounded = Physics.Raycast(center, down, GroundedRayLength, mLayerMaskEnvironment);
    }

    private void MovementHandler()
    {
        Vector3 velocity = mRigidbody.velocity;

        // Movement X-axis
        if (mWallJumpLockTimer < Time.time)
        {
            if (mInputDash > InputThreshold)
            {
                // Only dash in look direction
                float direction = Mathf.Sign(mLookDirection.x);

                if (mIsGrounded && !mDashTriggered)
                {
                    // Start dashing on the ground
                    mDashTimer = Time.time + DashDuration;
                    mDashTriggered = true;
                    velocity.x = direction * DashSpeed;
                    mInputHorizontal = direction;
                }
                else if (mIsGrounded && mDashTriggered && Time.time < mDashTimer)
                {
                    // Continue dashing on the ground
                    velocity.x = direction * DashSpeed;
                    mInputHorizontal = direction;
                }
                else if (!mIsGrounded && mDashTriggered)
                {
                    // Keep momentum while airborne
                    mInputHorizontal = direction;
                }
                else
                {
                    // Regular movement
                    velocity.x = mInputHorizontal * MoveSpeed;
                }
            }
            else
            {
                // Regular movement and reset dash trigger
                mDashTriggered = false;
                velocity.x = mInputHorizontal * MoveSpeed;
            }
        }

        // Movement Y-axis
        if (mInputJump > InputThreshold)
        {
            if (mIsGrounded && !mJumpTriggered)
            {
                // Jump from ground
                mJumpTimer = Time.time + JumpDuration;
                mJumpTriggered = true;
                velocity.y = JumpSpeed;
            }
            else if (CanWallJump() && !mJumpTriggered)
            {
                // Jump from wall and lock movement in X-axis for a duration
                mJumpTimer = Time.time + JumpDuration;
                mJumpTriggered = true;
                mWallJumpLockTimer = Time.time + WallJumpLockDuration;
                velocity.y = JumpSpeed;

                // Get horizontal momentum away from the wall
                float horizontal = -Mathf.Sign(mInputHorizontal);
                if (mInputDash > InputThreshold && !mDashTriggered)
                {
                    mDashTimer = Time.time + DashDuration;
                    mDashTriggered = true;
                    velocity.x = horizontal * DashSpeed;
                }
                else
                {
                    velocity.x = horizontal * MoveSpeed;
                }
            }
            else if (mJumpTriggered && Time.time < mJumpTimer)
            {
                // Increased jump from prolonged input
                velocity.y = JumpSpeed;
            }
        }
        else
        {
            // Reset jump trigger
            mJumpTriggered = false;
        }

        // Movement Z-axis
        velocity.z = 0;

        // Lock position.z
        Vector3 position = mRigidbody.position;
        position.z = 0;

        // Final movement
        mRigidbody.velocity = velocity;
        mRigidbody.position = position;
    }

    private void RotationHandler()
    {
        // Rotate player in look direction
        Vector3 lookDirection = mLookDirection;
        lookDirection.y = 0;
        lookDirection.Normalize();

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            mRigidbody.MoveRotation(targetRotation);
        }
    }

    private void ShootHandler()
    {
        if (mInputFire > InputThreshold && mShootTimer < Time.time)
        {
            mShootTimer = Time.time + ShootFireRate;
            mShootEffectTimer = Time.time + (ShootFireRate * ShootEffectTime);

            // Set up raycast properties
            mShootRay.origin = mWeapon.position;
            mShootRay.direction = mAimDirection;
            RaycastHit shootHit;

            // Play the effects
            mWeaponEffectAudio.Play();
            mWeaponEffectLight.enabled = true;
            mWeaponEffectLine.enabled = true;
            mWeaponEffectParticles.Stop();
            mWeaponEffectParticles.Play();

            // Set the first point for the line
            mWeaponEffectLine.SetPosition(0, mShootRay.origin);

            if (Physics.Raycast(mShootRay, out shootHit, ShootRange, mLayerMaskShootable, QueryTriggerInteraction.Ignore))
            {
                // If EnemyHealth component exist, then deal damage
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(ShootDamage, shootHit.point);
                }

                // Set the second point for the line at hitpoint
                mWeaponEffectLine.SetPosition(1, shootHit.point);
            }
            else
            {
                // Set the second point for the line at full range
                mWeaponEffectLine.SetPosition(1, mShootRay.origin + mShootRay.direction * ShootRange);
            }
        }

        if (mShootEffectTimer < Time.time)
        {
            // Stop the effects
            mWeaponEffectLight.enabled = false;
            mWeaponEffectLine.enabled = false;
        }
    }
}
