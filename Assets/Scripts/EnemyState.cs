using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum State
    {
        Attack,
        Idle
    }

    public State CurrentState;

    private void Start()
    { }

    private void OnDisable()
    {
        EventManager.StopListening("PlayerDeath", OnPlayerDeath);
    }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerDeath", OnPlayerDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentState = State.Attack;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentState = State.Idle;
        }
    }

    private void OnPlayerDeath()
    {
        CurrentState = State.Idle;
    }
}
