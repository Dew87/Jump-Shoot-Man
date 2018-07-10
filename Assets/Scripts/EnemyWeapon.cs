using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject ShootProjectile;
    public Transform ShootSpawn;
    public float ShootFireRate;

    private float mShootTimer;
    private EnemyState mEnemyState;
    private Rigidbody mPlayerRigidbody;

    private void Start()
    {
        mShootTimer = Time.time + ShootFireRate;
        mEnemyState = GetComponent<EnemyState>();
        mPlayerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch (mEnemyState.CurrentState)
        {
            case EnemyState.State.Attack:
                Shoot();
                break;
            default:
                break;
        }
    }

    private void Shoot()
    {
        if (mShootTimer < Time.time)
        {
            mShootTimer = Time.time + ShootFireRate;
            Vector3 position = ShootSpawn.position;
            Quaternion rotation = Quaternion.LookRotation(mPlayerRigidbody.position + mPlayerRigidbody.centerOfMass - position);
            Instantiate(ShootProjectile, position, rotation);
        }
    }
}
