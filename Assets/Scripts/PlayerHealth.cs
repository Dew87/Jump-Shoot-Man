using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int CurrentHealth;
    public int MaxHealth = 100;
    public float InvulnerableDuration = 0.5f;
    public Image DamageImage;
    public float DamageFlashSpeed = 5f;
    public Color DamageFlashColor = new Color(1f, 0f, 0f, 0.3f);
    public Text GameOverText;
    public Slider HealthSlider;

    public AudioClip[] HitAudioList;

    private float mInvulnerableTimer;

    private AudioSource mAudioSource;
    private PlayerController mPlayerController;
    private SpawnObjects mSpawnObjects;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        mInvulnerableTimer = 0f;

        mAudioSource = GetComponent<AudioSource>();
        mPlayerController = GetComponent<PlayerController>();
        mSpawnObjects = GetComponent<SpawnObjects>();
    }

    private void Update()
    {
        DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, Time.deltaTime * DamageFlashSpeed);
    }

    public void TakeDamage(int value, Vector3 hitpoint)
    {
        if (mInvulnerableTimer < Time.time)
        {
            mInvulnerableTimer = Time.time + InvulnerableDuration;
            CurrentHealth -= value;
            HealthSlider.value = CurrentHealth;
            DamageImage.color = DamageFlashColor;

            mPlayerController.Knockback(hitpoint);

            if (CurrentHealth <= 0)
            {
                Death();
            }
            else
            {
                // Play random hit audio
                int i = Random.Range(0, HitAudioList.Length);
                AudioClip audioClip = HitAudioList[i];
                mAudioSource.clip = audioClip;
                mAudioSource.Play();
            }
        }
    }

    public void GiveHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        HealthSlider.value = CurrentHealth;
    }

    private void Death()
    {
        GameOverText.gameObject.SetActive(true);
        HealthSlider.gameObject.SetActive(false);
        EventManager.TriggerEvent("PlayerDeath");
        mSpawnObjects.Spawn();
        Destroy(gameObject);
    }
}
