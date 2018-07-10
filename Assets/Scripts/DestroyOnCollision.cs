using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private SpawnObjects mSpawnObject;

    private void Start()
    {
        mSpawnObject = GetComponent<SpawnObjects>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (mSpawnObject != null)
        {
            mSpawnObject.Spawn();
        }
        Destroy(gameObject);
    }
}
