using UnityEngine;

public class SpawnObjectOnTrigger : MonoBehaviour
{
    public GameObject Object;
    public Transform ObjectTransform;
    public string TriggerTag;

    private void Start()
    {
        if (ObjectTransform == null)
        {
            ObjectTransform = transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggerTag))
        {
            Vector3 position = ObjectTransform.position;
            Quaternion rotation = ObjectTransform.rotation;
            Instantiate(Object, position, rotation);
        }
    }
}
