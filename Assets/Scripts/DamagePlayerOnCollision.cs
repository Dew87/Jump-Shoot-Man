using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    public int DamageOnContact = 1;

    private void Start()
    { }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollision(collision);
    }

    private void OnCollision(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            // Use first contact point as player hitpoint
            ContactPoint contact = collision.contacts[0];

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(DamageOnContact, contact.point);
        }
    }
}
