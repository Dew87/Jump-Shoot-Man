using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float Lifetime;

    private void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}
