using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    private PlayerAwarenessController _playerAwarenessController;

    private Vector2 _targetDirection;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;

    public LayerMask groundLayer; // Used for collision checking with walkable areas


    private float _randomDirectionChangeInterval = 2f; // Interval at which enemy changes direction
    private float _timeSinceLastDirectionChange; // Timer to track when to change direction

    private float nextDirectionChangeTime; // Time to track when to change direction next

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponentInChildren<PlayerAwarenessController>();

        _targetDirection = UnityEngine.Random.insideUnitCircle.normalized;
        nextDirectionChangeTime = Time.time + _randomDirectionChangeInterval; // Initialize the next direction change time
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            // Play death animation and then destroy the enemy after it's finished
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) // Ensure that the die animation isn't playing already
            {
                anim.SetTrigger("Die"); // Trigger death animation
                Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length); // Destroy after animation is complete
            }
            return; // Stop further updates if the enemy is dead
        }

        UpdateTargetDirection();
        Move();
        RotateTowardsTarget();
    }

    void Update()
    {
        if (health <= 0)
        {
            return; // Skip other updates when the enemy is dead
        }

        // Flip sprite based on movement direction
        if (_targetDirection.x != 0)
        {
            spriteRenderer.flipX = _targetDirection.x < 0;
        }

        // Change direction after a set interval
        if (Time.time >= nextDirectionChangeTime)
        {
            SetRandomTargetDirection();
        }
    }

    private void SetRandomTargetDirection()
    {
        // Set a new random direction
        _targetDirection = UnityEngine.Random.insideUnitCircle.normalized;
        nextDirectionChangeTime = Time.time + _randomDirectionChangeInterval; // Reset the direction change timer
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage!");
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        // Check if the target position is walkable (not blocked by obstacles in groundLayer)
        if (Physics2D.OverlapCircle(targetPos, 0.2f, groundLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void Move()
    {
        // Move the enemy smoothly towards the target direction
        Vector2 targetPos = rb.position + _targetDirection * _speed * Time.fixedDeltaTime;

        // Ensure the target position is walkable
        if (IsWalkable(targetPos))
        {
            // Smoothly move the enemy to the target position using MoveTowards to avoid jitter
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, _speed * Time.fixedDeltaTime));
        }
        else
        {
            // If blocked, change direction and try again
            SetRandomTargetDirection();
        }
    }

    private void UpdateTargetDirection()
    {
        // Check if the enemy is aware of the player (if true, move towards the player)
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            // Otherwise, the enemy moves in a random direction
            _timeSinceLastDirectionChange += Time.deltaTime;
            if (_timeSinceLastDirectionChange >= _randomDirectionChangeInterval)
            {
                Vector2 newDirection;
                do
                {
                    newDirection = UnityEngine.Random.insideUnitCircle.normalized; // Generate a random direction
                }
                while (!IsWalkable((Vector2)transform.position + newDirection)); // Ensure it's walkable

                _targetDirection = newDirection;
                _timeSinceLastDirectionChange = 0f; // Reset the timer
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return; // Do nothing if there's no target direction
        }

        // Rotate the enemy to face the direction it's moving towards
        float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.SetRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime));
    }
}
