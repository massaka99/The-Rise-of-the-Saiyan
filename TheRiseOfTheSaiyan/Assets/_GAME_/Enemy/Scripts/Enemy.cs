using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;

    [SerializeField] private GameObject healthBarPrefab;
    private FloatingHealthBar healthBarInstance;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 360f;

    public LayerMask groundLayer;
    public LayerMask obstacleLayer; 

    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;

    private float randomDirectionChangeInterval = 2f;
    private float timeSinceLastDirectionChange;
    private bool isKnockedBack = false;

    [Header("Attack Parameters")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private AudioClip attackSound;

    private float nextAttackTime;
    private bool canAttack = true;

    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponentInChildren<PlayerAwarenessController>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.maxDistance = 20f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }

        health = maxHealth;
    }

    private void Start()
    {
        moveDirection = UnityEngine.Random.insideUnitCircle.normalized;

        if (healthBarInstance == null && healthBarPrefab != null)
        {
            GameObject healthBarCanvas = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
            healthBarInstance = healthBarCanvas.GetComponentInChildren<FloatingHealthBar>();

            healthBarInstance.UpdateHealthBar(health, maxHealth);
            healthBarInstance.AssignTarget(transform);
        }
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            HandleDeath();
            return;
        }

        if (!isKnockedBack)
        {
            UpdateTargetDirection();
            
            // Only move if not within attack range
            if (!_playerAwarenessController.WithinAttackRange)
            {
                Move();
            }
            else
            {
                TryAttackPlayer();
            }
            
            RotateTowardsTarget();
        }
    }

    private void Update()
    {
        if (health <= 0) return;

        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }

        timeSinceLastDirectionChange += Time.deltaTime;
        if (timeSinceLastDirectionChange >= randomDirectionChangeInterval)
        {
            SetRandomDirection();
        }
    }

    private void SetRandomDirection()
    {
        moveDirection = UnityEngine.Random.insideUnitCircle.normalized;
        timeSinceLastDirectionChange = 0f;
    }

    public void TakeDamage(float damage, Vector2? knockbackDirection = null)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else if (knockbackDirection.HasValue)
        {
            Knockback(knockbackDirection.Value);
        }

        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealthBar(health, maxHealth);
        }
    }

    private void Knockback(Vector2 direction)
    {
        if (rb != null)
        {
            float knockbackForce = 5f;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void EndKnockback()
    {
        isKnockedBack = false;
    }

    private void Die()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (gameObject.CompareTag("Level2Boss"))
            {
                // Determine which boss was defeated based on name
                if (gameObject.name.Contains("Frieza"))
                {
                    QuestManager.Instance?.SetBossDefeated(1);
                    Debug.Log("Frieza has been defeated! Cell quest begins.");
                }
                else if (gameObject.name.Contains("Cell"))
                {
                    QuestManager.Instance?.SetBossDefeated(2);
                    Debug.Log("Cell has been defeated! Buu quest begins.");
                }
                else if (gameObject.name.Contains("Buu"))
                {
                    QuestManager.Instance?.SetBossDefeated(3);
                    Debug.Log("Buu has been defeated! All bosses are vanquished!");
                }
            }
            else if (gameObject.CompareTag("Beerus"))
            {
                QuestManager.Instance?.CompleteBeerusQuest();
                Debug.Log("Beerus has been defeated! Game Complete!");
            }

            anim.SetTrigger("Die");
            
            if (healthBarInstance != null)
            {
                Destroy(healthBarInstance.gameObject);
            }

            if (SceneManager.GetActiveScene().buildIndex == 3 && gameObject.CompareTag("Level2Saibaman"))
            {
                QuestManager.Instance?.IncrementLevel2SaibamenKilled();
                Debug.Log("Level 2 Saibaman killed!");
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                QuestManager.Instance?.IncrementSaibamenKilled();
                Debug.Log("Saibaman killed!");
            }
            else if (gameObject.CompareTag("Vegeta"))
            {
                QuestManager.Instance?.CompleteVegetaQuest();
                Debug.Log("Vegeta has been defeated!");
            }

            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, groundLayer) == null &&
               Physics2D.OverlapCircle(targetPos, 0.2f, obstacleLayer) == null;
    }

    private void Move()
    {
        Vector2 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        if (IsWalkable(targetPos))
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime));
        }
        else
        {
            SetRandomDirection(); 
        }
    }

    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController != null && _playerAwarenessController.AwareOfPlayer)
        {
            moveDirection = _playerAwarenessController.DirectionToPlayer.normalized;
        }
    }

    private void RotateTowardsTarget()
    {
        if (moveDirection == Vector2.zero) return;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.SetRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime));
    }

    private void TryAttackPlayer()
{
    if (!canAttack || Time.time < nextAttackTime) return;

    float distanceToPlayer = Vector2.Distance(transform.position, _playerAwarenessController.PlayerTransform.position);
    
    if (distanceToPlayer <= attackRange)
    {
        // Trigger attack animation
        anim.SetTrigger("Attack");
        
        // Play attack sound
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Deal damage to player
        var playerHealth = _playerAwarenessController.PlayerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        nextAttackTime = Time.time + attackCooldown;
    }
}

    private void HandleDeath()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (gameObject.CompareTag("Level2Boss"))
            {
                // Determine which boss was defeated based on name
                if (gameObject.name.Contains("Frieza"))
                {
                    QuestManager.Instance?.SetBossDefeated(1);
                    Debug.Log("Frieza has been defeated! Cell quest begins.");
                }
                else if (gameObject.name.Contains("Cell"))
                {
                    QuestManager.Instance?.SetBossDefeated(2);
                    Debug.Log("Cell has been defeated! Buu quest begins.");
                }
                else if (gameObject.name.Contains("Buu"))
                {
                    QuestManager.Instance?.SetBossDefeated(3);
                    Debug.Log("Buu has been defeated! All bosses are vanquished!");
                }
            }

            anim.SetTrigger("Die");
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
