using UnityEngine;

public class Mover : MonoBehaviour
{
    public float Speed = 1f;

    private Rigidbody mRigidbody;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();

        Vector3 velocity = transform.forward * Speed;
        mRigidbody.velocity = velocity;
    }
}
