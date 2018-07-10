using UnityEngine;

public class EnemyMovementAir : MonoBehaviour
{
    public bool InitialMovement = false;
    public float Acceleration = 1f;
    public float AngularSpeed = 1f;
    public float IdleAngularDrag = 0f;
    public float IdleDrag = 0f;
    public float Speed = 1f;

    private EnemyState mEnemyState;
    private Rigidbody mPlayerRigidbody;
    private Rigidbody mRigidbody;

    private void Start()
    {
        mEnemyState = GetComponent<EnemyState>();
        mPlayerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        mRigidbody = GetComponent<Rigidbody>();

        if (InitialMovement)
        {
            Vector3 velocity = transform.forward * Speed;
            mRigidbody.velocity = velocity;
        }
    }

    private void FixedUpdate()
    {
        switch (mEnemyState.CurrentState)
        {
            case EnemyState.State.Attack:
                MovementHandler();
                RotationHandler();
                break;
            case EnemyState.State.Idle:
                IdleHandler();
                break;
            default:
                break;
        }
    }

    private void IdleHandler()
    {
        // Set idle drag
        mRigidbody.angularDrag = IdleAngularDrag;
        mRigidbody.drag = IdleDrag;
    }

    private void MovementHandler()
    {
        // Remove drag
        mRigidbody.angularDrag = 0f;
        mRigidbody.drag = 0f;

        // Seek behaviour to player position
        Vector3 velocity = mRigidbody.velocity;
        Vector3 direction = mPlayerRigidbody.position + mPlayerRigidbody.centerOfMass - mRigidbody.position;
        Vector3 desired = direction.normalized * Speed;
        Vector3 steering = desired - velocity;

        mRigidbody.AddForce(steering.normalized * Acceleration * mRigidbody.mass * Time.deltaTime);
    }

    private void RotationHandler()
    {
        Vector3 direction = mPlayerRigidbody.position + mPlayerRigidbody.centerOfMass - mRigidbody.position;
        mRigidbody.rotation = Quaternion.Slerp(mRigidbody.rotation, Quaternion.LookRotation(direction), AngularSpeed * Time.deltaTime);
    }
}
