using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    public float Smoothing = 5f;

    private Vector3 mOffset;
    private Transform mPlayerTransform;

    private void Start()
    {
        mPlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mOffset = transform.position - mPlayerTransform.position;
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
        Vector3 targetCameraPosition = mPlayerTransform.position + mOffset;

        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * Smoothing);
    }

    private void OnPlayerDeath()
    {
        this.enabled = false;
    }
}
