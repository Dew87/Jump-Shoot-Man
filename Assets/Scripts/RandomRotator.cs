using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float Tumble;

    private Rigidbody mRigidbody;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();

        mRigidbody.angularVelocity = Random.insideUnitSphere * Tumble;
    }
}
