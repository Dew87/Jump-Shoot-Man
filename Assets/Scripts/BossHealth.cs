using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth
{
    public int CurrentArmor;
    public int MaxArmor = 10;
    public Text GameOverText;
    public Slider HealthSlider;
    public float InvulnerableDuration = 2.0f;

    public AudioClip[] HitArmorAudioList;

    private float InvulnerableTimer;

    private BossController mBossController;

    protected override void Start()
    {
        base.Start();
        CurrentArmor = MaxArmor;
        InvulnerableTimer = 0f;

        mBossController = GetComponent<BossController>();
    }

    private void OnEnable()
    {
        HealthSlider.gameObject.SetActive(true);
    }

    public override void TakeDamage(int value, Vector3 hitpoint)
    {
        if (InvulnerableTimer < Time.time)
        {
            CurrentArmor -= value;

            if (CurrentArmor <= 0)
            {
                InvulnerableTimer = Time.time + InvulnerableDuration;
                base.TakeDamage(1, hitpoint);
                HealthSlider.value = CurrentHealth;
                mBossController.Knockback(hitpoint);
                CurrentArmor = MaxArmor;
            }
            else
            {
                // Play random hit audio
                int i = Random.Range(0, HitArmorAudioList.Length);
                AudioClip audioClip = HitArmorAudioList[i];
                mAudioSource.clip = audioClip;
                mAudioSource.Play();

                // Play particle effect
                mParticleHit.transform.position = hitpoint;
                mParticleHit.Play();
            }
        }
    }

    protected override void Death()
    {
        GameOverText.gameObject.SetActive(true);
        HealthSlider.gameObject.SetActive(false);
        mSpawnObjects.Spawn();
        Destroy(gameObject);
    }
}
