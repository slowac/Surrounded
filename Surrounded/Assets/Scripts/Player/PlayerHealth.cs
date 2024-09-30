using slowac_UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;//Maximum health value
    [SerializeField] private Slider healthbarSlider; // Health bar slider
    [SerializeField] private Image healthbarFillImage; // Health bar fill image
    [SerializeField] private Color maxHealthColor; // Maximum health value color
    [SerializeField] private Color zeroHealthColor; // Zero health value color
    [SerializeField] private GameObject damageTextPrefab; // Damage text prefab
    [SerializeField] private AudioClip healingAudioClip; // Healing sound effect

    private int currentHealth; // Current health value
    private AudioSource audioSource; // Audio source component

    private int healingAmount = 1; // Health value restored per second
    private float healingTimer = 10f; // Health recovery timer
    private float lastDamageTime; // Time of last damage
    private float healingAccumulator = 0f;

    private void Start()
    {
        currentHealth = 100; // Initialize current health
        SetHealthbarUi(); // Set health bar UI

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Time.time - lastDamageTime >= healingTimer && currentHealth < (int)maxHealth)
        {
            Heal(healingAmount * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = healingAudioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (audioSource.clip == healingAudioClip && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        }

        public void DealDamage(int damage)
        {
            currentHealth -= damage; // deduct damage value
            lastDamageTime = Time.time; // update the time of last damage

            CheckIfDead(); // check if dead
            SetHealthbarUi(); // update health bar UI
        }

        private void Heal(float amount)
        {
            healingAccumulator += amount;
            int healingToApply = Mathf.FloorToInt(healingAccumulator);
            if (healingToApply >= 1)
            {
                currentHealth = Mathf.Min((int)maxHealth, currentHealth + healingToApply);
                healingAccumulator -= healingToApply;
            SetHealthbarUi();
        }
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // destroy object
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); // Calculate health percentage
        healthbarSlider.value = healthPercentage; // Set slider value
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / maxHealth); // Set fill color
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp(((float)currentHealth / maxHealth) * 100, 0f, maxHealth); // Calculate health percentage formula
    }
}