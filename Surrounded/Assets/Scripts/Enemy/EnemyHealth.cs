using slowac_UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] public string enemyName; // enemy name
    [SerializeField] private float maxHealth; // maximum health value
    [SerializeField] private Slider healthbarSlider; // health bar slider
    [SerializeField] private Image healthbarFillImage; // health bar fill image
    [SerializeField] private Color maxHealthColor; // maximum health value color
    [SerializeField] private Color zeroHealthColor; // zero health value color
    [SerializeField] private GameObject damageTextPrefab; // damage text prefab
    [SerializeField] private int enemyScore = 1000; // enemy score value
    [SerializeField] private float knockbackMultiplier ; // ratio between damage and knockback force
    [SerializeField] private float minKnockbackForce ; // Minimum knockback force
    [SerializeField] private float maxKnockbackForce ; // Maximum knockback force

    public KillEffectController killEffectController;
    public ParticleSystem deathParticleEffect; // Ölüm efekti için ParticleSystem referansı

    private float currentHealth; // Current health value
    public EnemyAI enemyAI; // Enemy's EnemyAI component
    private Rigidbody enemyRigidbody; // Enemy's Rigidbody component

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health value
        SetHealthbarUi(); // Set health bar UI
        killEffectController = FindObjectOfType<KillEffectController>();
        enemyRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody component
        enemyAI = GetComponent<EnemyAI>(); // Get EnemyAI component
    }

    public void DealDamage(int damage, Vector3 originPosition)
    {
        // Deduct damage value
        currentHealth -= damage;

        // Instantiate damage text and initialize
        Instantiate(damageTextPrefab, transform.position, Quaternion.identity).GetComponent<DamageText>().Initialise(damage);

        // Calculate knockback direction
        Vector3 knockbackDirection = (transform.position - originPosition).normalized;

        // Calculate knockback force
        float knockbackMagnitude = Mathf.Clamp(damage * knockbackMultiplier, minKnockbackForce, maxKnockbackForce);

        // Calculate upward force
        Vector3 upwardForce = Vector3.up * knockbackMagnitude*0.2f; // Take 20% of the knockback force
        // Calculate backward force
        Vector3 backwardForce = knockbackDirection * knockbackMagnitude * 0.8f; // Take 80% of the knockback force

        // Calculate final knockback force
        Vector3 knockbackForce = upwardForce + backwardForce;

        // Knockback using calculated force
        StartCoroutine(ApplyKnockback(knockbackForce));

        CheckIfDead();
        // Update health bar UI
        SetHealthbarUi();
    }

    // Create coroutine to perform knockback effect
    private IEnumerator ApplyKnockback(Vector3 knockbackForce)
    {
        float knockbackStartTime = Time.time;
        float knockbackDuration = 1f;

        enemyAI.isKnockedBack = true; // When knockback starts, set isKnockedBack to true
        enemyRigidbody.isKinematic = false; // Set Rigidbody to non-Kinematic

        // Second, apply knockback force
        enemyRigidbody.AddForce(knockbackForce, ForceMode.Impulse);

        while (Time.time < knockbackStartTime + knockbackDuration)
        {
            yield return null;
        }

        enemyRigidbody.velocity = Vector3.zero; // Clear any remaining velocity
        enemyRigidbody.isKinematic = true; // Set Rigidbody to Kinematic
        enemyAI.isKnockedBack = false; // When the knockback ends, set isKnockedBack to false
    }

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            LevelManager.instance.EnemyDefeated(); // Notify GameManager that the enemy has been defeated
            ScoreUI scoreUI = FindObjectOfType<ScoreUI>(); // Get a reference to the ScoreUI component
            if (scoreUI != null)
            {
                scoreUI.AddScore(enemyScore); // Add score when the enemy dies
            }
            ParticleSystem effect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity); // Ölüm efektini oluştur
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax); // Efekti, süresi dolduktan sonra yok et
            Destroy(gameObject); // Destroy the object
            killEffectController.TriggerKillEffect();
            Debug.Log("Destroy!");
        }
    }

    private void SetHealthbarUi()
    {
        float healthPercentage = CalculateHealthPercentage(); // Calculate health percentage
        healthbarSlider.value = healthPercentage; // Set slider value
        healthbarFillImage.color = Color.Lerp(zeroHealthColor, maxHealthColor, healthPercentage / 100f); // Set fill color
    }

    private float CalculateHealthPercentage()
    {
        return Mathf.Clamp((currentHealth / maxHealth) * 100, 0f, 100f); // Calculate health percentage formula
    }
}