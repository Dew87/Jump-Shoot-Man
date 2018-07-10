using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum State
    {
        Jump,
        Decide,
        Idle,
        ShootMissiles,
        ShootProjectiles
    }

    public State CurrentState;
    public float DecideDuration = 2.0f;
    public float IdleAngularDrag = 0f;
    public float IdleDrag = 0f;
    public float KnockbackDuration = 0.3f;
    public float KnockbackSpeed = 8f;
    public GameObject Missile;
    public float MissileFireRate = 1.0f;
    public int   MissileVolleys = 3;
    public Transform[] MissileSpawns;
    public GameObject Projectile;
    public float ProjectileFireRate = 0.5f;
    public int ProjectileVolleys = 6;
    public Transform ProjectileSpawn;

    private float mKnockbackTimer;
    private int   mStateCounter;
    private float mStateTimer;

    private Rigidbody mPlayerRigidbody;
    private Rigidbody mRigidbody;

    private void Start()
    {
        mKnockbackTimer = 0f;
        mStateCounter = 0;
        mStateTimer = 0f;

        mPlayerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        mRigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", OnPlayerDeath);
    }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerDeath", OnPlayerDeath);
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Decide:
                DecideHandler();
                break;
            case State.Idle:
                IdleHandler();
                break;
            case State.Jump:
                JumpHandler();
                break;
            case State.ShootMissiles:
                ShootMissilesHandler();
                break;
            case State.ShootProjectiles:
                ShootProjectilesHandler();
                break;
            default:
                break;
        }
    }

    public void Knockback(Vector3 hitpoint)
    {
        mKnockbackTimer = Time.time + KnockbackDuration;

        Vector3 direction = mRigidbody.position + mRigidbody.centerOfMass - hitpoint;
        direction.Normalize();

        mRigidbody.velocity = direction * KnockbackSpeed;

        CurrentState = State.Decide;
        mStateTimer = Time.time + DecideDuration;
        mStateCounter = 0;
    }


    private void DecideHandler()
    {
        if (mStateTimer < Time.time)
        {
            int i = Random.Range(1, 3);
            switch (i)
            {
                case 0:
                    CurrentState = State.Jump;
                    mStateTimer = Time.time;
                    mStateCounter = 0;
                    break;
                case 1:
                    CurrentState = State.ShootMissiles;
                    mStateTimer = Time.time;
                    mStateCounter = 0;
                    break;
                case 2:
                    CurrentState = State.ShootProjectiles;
                    mStateTimer = Time.time;
                    mStateCounter = 0;
                    break;
                default:
                    break;
            }
        }
    }

    private void IdleHandler()
    {
        // Set idle drag
        mRigidbody.angularDrag = IdleAngularDrag;
        mRigidbody.drag = IdleDrag;
    }

    private void JumpHandler()
    {
        // TODO: Jump towards/away from player position
    }

    private void OnPlayerDeath()
    {
        CurrentState = State.Idle;
    }

    private void ShootMissilesHandler()
    {
        if (mStateTimer < Time.time)
        {
            mStateTimer = Time.time + MissileFireRate;
            foreach (Transform missileTransform in MissileSpawns)
            {
                Vector3 position = missileTransform.position;
                Quaternion rotation = missileTransform.rotation;
                Instantiate(Missile, position, rotation);
            }

            mStateCounter++;
            if (mStateCounter >= MissileVolleys)
            {
                CurrentState = State.Decide;
                mStateTimer = Time.time + DecideDuration;
                mStateCounter = 0;
            }
        }
    }

    private void ShootProjectilesHandler()
    {
        if (mStateTimer < Time.time)
        {
            mStateTimer = Time.time + ProjectileFireRate;
            Vector3 position = ProjectileSpawn.position;
            Quaternion rotation = Quaternion.LookRotation(mPlayerRigidbody.position + mPlayerRigidbody.centerOfMass - position);
            Instantiate(Projectile, position, rotation);

            mStateCounter++;
            if (mStateCounter >= ProjectileVolleys)
            {
                CurrentState = State.Decide;
                mStateTimer = Time.time + DecideDuration;
                mStateCounter = 0;
            }
        }
    }
}
