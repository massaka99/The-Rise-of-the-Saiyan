using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

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

        CreateOrUpdateHealthBar();
    }

    private void CreateOrUpdateHealthBar()
    {
        FloatingHealthBar[] existingHealthBars = FindObjectsOfType<FloatingHealthBar>();
        
        foreach (var healthBar in existingHealthBars)
        {
            Destroy(healthBar.gameObject);
        }

        if (healthBarPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject healthBarObj = Instantiate(healthBarPrefab, canvas.transform);
                healthBarInstance = healthBarObj.GetComponentInChildren<FloatingHealthBar>();
                healthBarInstance.AssignTarget(transform);
                healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentHealth = maxHealth;
        isDead = false;
        
        StartCoroutine(InitializeHealthBarNextFrame());
    }

    private System.Collections.IEnumerator InitializeHealthBarNextFrame()
    {
        yield return null;
        CreateOrUpdateHealthBar();
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
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
        
        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
        }

        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            animator?.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        
        animator?.SetTrigger("Die");
        
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        GetComponent<Player_Controller>().enabled = false;
        GetComponent<Attack>().enabled = false;

        onPlayerDeath?.Invoke();

        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
} 