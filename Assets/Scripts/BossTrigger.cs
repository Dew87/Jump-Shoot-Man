using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Door;

    private Animator mAnimatorDoor;
    private AudioSource mAudioDoorClose;
    private Collider mColliderTrigger;

    private void Start()
    {
        mAnimatorDoor = Door.GetComponent<Animator>();
        mAudioDoorClose = GetComponent<AudioSource>();
        mColliderTrigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activate the boss
            Boss.SetActive(true);

            // Close the door
            mAnimatorDoor.SetTrigger("Close");
            mAudioDoorClose.Play();

            // Deactivate collider so trigger happens only once
            mColliderTrigger.enabled = false;

            // Destroy game object after playing sound
            Destroy(gameObject, mAudioDoorClose.clip.length);
        }
    }
}
