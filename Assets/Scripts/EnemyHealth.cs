using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int CurrentHealth;
    public int MaxHealth = 1;

    public AudioClip[] HitAudioList;

    protected AudioSource mAudioSource;
    protected SpawnObjects mSpawnObjects;
    protected ParticleSystem mParticleHit;

    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;

        mAudioSource = GetComponent<AudioSource>();
        mSpawnObjects = GetComponent<SpawnObjects>();
        mParticleHit = transform.Find("HitEffect").GetComponent<ParticleSystem>();
   }

    public virtual void TakeDamage(int value, Vector3 hitpoint)
    {
        CurrentHealth -= value;

        // Play random hit audio
        int i = Random.Range(0, HitAudioList.Length);
        AudioClip audioClip = HitAudioList[i];
        mAudioSource.clip = audioClip;
        mAudioSource.Play();

        // Play particle effect
        mParticleHit.transform.position = hitpoint;
        mParticleHit.Play();

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        if (mSpawnObjects != null)
        {
            mSpawnObjects.Spawn();
        }
        Destroy(gameObject);
    }
}
