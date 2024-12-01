using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyKilled;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;

    [SerializeField] private GameObject healthBarPrefab;  // Reference to the health bar prefab (which includes Canvas and Slider)
    private FloatingHealthBar healthBarInstance;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 360f;

    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;

    public LayerMask groundLayer;

    private float randomDirectionChangeInterval = 2f;
    private float timeSinceLastDirectionChange;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponentInChildren<PlayerAwarenessController>();

        health = maxHealth; // Initialize health
    }

    private void Start()
    {
        moveDirection = UnityEngine.Random.insideUnitCircle.normalized;

        // Check if healthBarInstance is not already assigned.
        // Only instantiate a new health bar if it's not already assigned.
        if (healthBarInstance == null && healthBarPrefab != null)
        {
            // Instantiate the Canvas with the health bar and assign it
            GameObject healthBarCanvas = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
            healthBarInstance = healthBarCanvas.GetComponentInChildren<FloatingHealthBar>();

            // Set the health bar's initial health values
            healthBarInstance.UpdateHealthBar(health, maxHealth);

            // Set the position of the health bar above the enemy
            healthBarCanvas.transform.position = transform.position + new Vector3(0, 1, 0); // Adjust offset as needed
        }
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                anim.SetTrigger("Die");
                Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length); // Destroy after animation
            }
            return;
        }

        UpdateTargetDirection();
        Move();
        RotateTowardsTarget();
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

        // Update the health bar's position (keep it above the enemy)
        if (healthBarInstance != null)
        {
            healthBarInstance.transform.position = transform.position + new Vector3(0, 1, 0);
        }
    }

    private void SetRandomDirection()
    {
        moveDirection = UnityEngine.Random.insideUnitCircle.normalized;
        timeSinceLastDirectionChange = 0f;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealthBar(health, maxHealth); // Update the health bar when damage is taken
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        // Destroy the health bar when the enemy dies
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

        // Notify any listeners
        OnEnemyKilled?.Invoke(this);

        // Destroy the enemy object
        Destroy(gameObject, 0.5f);
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, groundLayer) == null;
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
}
