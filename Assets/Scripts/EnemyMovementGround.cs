using UnityEngine;

public class EnemyMovementGround : MonoBehaviour
{
    private Animator mAnimator;
    private EnemyState mEnemyState;
    private UnityEngine.AI.NavMeshAgent mNavMeshAgent;
    private Transform mPlayer;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        mEnemyState = GetComponent<EnemyState>();
        mNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        switch (mEnemyState.CurrentState)
        {
            case EnemyState.State.Attack:
                mNavMeshAgent.isStopped = false;
                mNavMeshAgent.SetDestination(mPlayer.position);
                break;
            case EnemyState.State.Idle:
                mNavMeshAgent.isStopped = true;
                break;
            default:
                break;
        }

        float speed = mNavMeshAgent.velocity.magnitude / mNavMeshAgent.speed;
        mAnimator.SetFloat("Movement", speed);
    }
}
