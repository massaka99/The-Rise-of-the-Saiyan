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

            // If the enemy is within attack range, stop walking and attack
            if (_playerAwarenessController.WithinAttackRange)
            {
                anim.SetBool("IsWalking", false); // Stop walking animation
                anim.SetBool("IsAttacking", true); // Start attacking animation
                TryAttackPlayer();
            }
            else
            {
                anim.SetBool("IsAttacking", false); // Stop attacking animation
                Move(); // Continue moving if not attacking
            }
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

        // Update Animator with movement values for Blend Tre
        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);

        // Ensure attacking resets if not within attack range
        if (!_playerAwarenessController.WithinAttackRange)
        {
            anim.SetBool("IsAttacking", false);
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
                if (gameObject.name.Contains("Frieza") || gameObject.name.Contains("Cell") || gameObject.name.Contains("Buu"))
                {
                    // Handled by ScriptedEnemyAI
                    return;
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
            anim.SetBool("IsWalking", true); // Enable walking animation

            // Pass movement direction to Animator for Blend Tree
            anim.SetFloat("moveX", moveDirection.x);
            anim.SetFloat("moveY", moveDirection.y);

            // Flip sprite based on movement direction
            if (moveDirection.x != 0)
            {
                spriteRenderer.flipX = moveDirection.x < 0;
            }
        }
        else
        {
            SetRandomDirection();
            anim.SetBool("IsWalking", false); // Stop walking animation
        }
    }



    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController != null && _playerAwarenessController.AwareOfPlayer)
        {
            moveDirection = _playerAwarenessController.DirectionToPlayer.normalized;
        }
    }

    private void TryAttackPlayer()
    {
        if (!canAttack || Time.time < nextAttackTime)
        {
            // Set IsAttacking to false when not attacking
            anim.SetBool("IsAttacking", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _playerAwarenessController.PlayerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            // Stop movement when attacking
            rb.velocity = Vector2.zero;

            // Trigger attack animation
            anim.SetTrigger("Attack");
            anim.SetBool("IsAttacking", true); // Set attacking state

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
        else
        {
            // Set IsAttacking to false when player is out of range
            anim.SetBool("IsAttacking", false);
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
