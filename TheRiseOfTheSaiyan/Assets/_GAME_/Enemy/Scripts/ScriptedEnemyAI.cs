using System;
using UnityEngine;

public class ScriptedEnemyAI : MonoBehaviour
{
    public static event Action<ScriptedEnemyAI> OnEnemyKilled;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f; 
    private float currentHealth;                    
    [SerializeField] private GameObject healthBarPrefab; 
    private FloatingHealthBar healthBarInstance;         

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float walkTime = 3f;

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private AudioClip attackSound;

    [Header("Detection Settings")]
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private PlayerAwarenessController _playerAwarenessController;

    private Vector2 moveDirection;
    private float stateTimer;
    private float nextAttackTime;

    private enum AIState { Idle, Walking, Chasing }
    private AIState currentState = AIState.Idle;

    private void Awake()
    {
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
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarInstance == null && healthBarPrefab != null)
        {
            GameObject healthBarCanvas = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
            healthBarInstance = healthBarCanvas.GetComponentInChildren<FloatingHealthBar>();

            healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
            healthBarInstance.AssignTarget(transform);
        }

        SetIdleState();
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        if (_playerAwarenessController.AwareOfPlayer)
        {
            currentState = AIState.Chasing;
        }

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.Walking:
                HandleWalkingState();
                break;
            case AIState.Chasing:
                HandleChasingState();
                break;
        }
    }

    private void SetIdleState()
    {
        currentState = AIState.Idle;
        stateTimer = idleTime;
        anim.SetBool("IsWalking", false);
    }

    private void SetWalkingState()
    {
        currentState = AIState.Walking;
        stateTimer = walkTime;
        moveDirection = UnityEngine.Random.insideUnitCircle.normalized;
        anim.SetBool("IsWalking", true);
        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);
    }

    private void HandleIdleState()
    {
        if (stateTimer <= 0)
        {
            SetWalkingState();
        }
    }

    private void HandleWalkingState()
    {
        if (stateTimer <= 0)
        {
            SetIdleState();
            return;
        }

        Vector2 targetPos = rb.position + moveDirection * moveSpeed * Time.deltaTime;

        if (IsWalkable(targetPos))
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime));
        }
        else
        {
            SetIdleState();
        }

        if (moveDirection.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0;
        }

        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);
    }

    private void HandleChasingState()
    {
        if (!_playerAwarenessController.AwareOfPlayer)
        {
            SetIdleState();
            return;
        }

        Vector2 directionToPlayer = _playerAwarenessController.DirectionToPlayer.normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, _playerAwarenessController.PlayerTransform.position);

        if (distanceToPlayer > attackRange)
        {
            // Move towards the player
            rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", true);
        }
        else
        {
            // Stop moving when close enough to attack
            rb.velocity = Vector2.zero;
            anim.SetBool("IsWalking", false);

            // Attempt to attack the player
            TryAttackPlayer();
        }

        // Flip sprite based on movement direction
        if (directionToPlayer.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = directionToPlayer.x < 0;
        }

        anim.SetFloat("moveX", directionToPlayer.x);
        anim.SetFloat("moveY", directionToPlayer.y);
    }

    private void TryAttackPlayer()
    {
        if (Time.time < nextAttackTime) return;

        Vector2 directionToPlayer = (_playerAwarenessController.PlayerTransform.position - transform.position).normalized;

        // Set direction parameters
        anim.SetFloat("moveX", directionToPlayer.x);
        anim.SetFloat("moveY", directionToPlayer.y);

        // Trigger attack
        anim.SetTrigger("Attack");

        // Play attack sound
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Check for player damage
        if (Vector2.Distance(transform.position, _playerAwarenessController.PlayerTransform.position) <= attackRange)
        {
            var playerHealth = _playerAwarenessController.PlayerTransform.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        nextAttackTime = Time.time + attackCooldown;
    }




    public void TakeDamage(float damage, Vector2? knockbackDirection = null)
    {
        currentHealth -= damage;

        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (knockbackDirection.HasValue)
        {
            Knockback(knockbackDirection.Value);
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
            else if (gameObject.CompareTag("Vegeta"))
            {
                QuestManager.Instance?.CompleteVegetaQuest();
                Debug.Log("Vegeta has been defeated!");
            }
            else if (gameObject.CompareTag("Beerus"))
            {
                QuestManager.Instance?.CompleteBeerusQuest();
                Debug.Log("Beerus has been defeated! Game Complete!");
            }

            // Trigger the death animation
            anim.SetTrigger("Die");

            // Destroy health bar instance if present
            if (healthBarInstance != null)
            {
                Destroy(healthBarInstance.gameObject);
            }

            // Invoke death event and destroy the enemy object
            OnEnemyKilled?.Invoke(this);
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }


    private bool IsWalkable(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, groundLayer) == null &&
               Physics2D.OverlapCircle(targetPos, 0.2f, obstacleLayer) == null;
    }
}
