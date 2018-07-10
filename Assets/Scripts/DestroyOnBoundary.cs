using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    private void Start()
    { }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
