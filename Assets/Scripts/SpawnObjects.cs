using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectStruct
    {
        public float Chance;
        public GameObject Object;
        public Vector3 Offset;
    }

    public ObjectStruct[] ObjectList;

    private void Start()
    { }

    public void Spawn()
    {
        foreach (ObjectStruct objectStruct in ObjectList)
        {
            float random = Random.value;
            if (random <= objectStruct.Chance)
            {
                Vector3 position = transform.position + objectStruct.Offset;
                Quaternion rotation = transform.rotation;
                Instantiate(objectStruct.Object, position, rotation);
            }
        }
    }
}
