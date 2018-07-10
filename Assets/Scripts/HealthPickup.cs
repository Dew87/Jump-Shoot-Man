using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int Health = 1;

    private SpawnObjects mSpawnObject;

    private void Start()
    {
        mSpawnObject = GetComponent<SpawnObjects>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.GiveHealth(Health);
            mSpawnObject.Spawn();
            Destroy(gameObject);
        }
    }
}
