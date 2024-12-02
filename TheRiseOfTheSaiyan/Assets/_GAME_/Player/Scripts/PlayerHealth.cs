using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float invincibilityDuration = 1f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Health Bar")]
    [SerializeField] private GameObject healthBarPrefab;
    private FloatingHealthBar healthBarInstance;

    private AudioSource audioSource;
    private bool isInvincible;
    private float invincibilityTimer;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead;

    public UnityEvent onPlayerDeath;
    public UnityEvent<float> onHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isDead = false;

        // Create health bar
        if (healthBarPrefab != null)
        {
            GameObject healthBarObj = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
            healthBarInstance = healthBarObj.GetComponentInChildren<FloatingHealthBar>();
            healthBarInstance.AssignTarget(transform);
            healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            // Flash effect
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.time * 10, 1f));
            
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || isDead) return;

        currentHealth -= damage;
        
        // Update health bar
        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
        }

        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Start invincibility
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            // Hit animation
            animator?.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        
        // Play death animation
        animator?.SetTrigger("Die");
        
        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Disable player controls
        GetComponent<Player_Controller>().enabled = false;
        GetComponent<Attack>().enabled = false;

        onPlayerDeath?.Invoke();

        // Optional: Restart level after delay
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
} 