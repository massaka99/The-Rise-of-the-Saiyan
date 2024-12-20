using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1f;         
    public int damage = 10;               
    public float attackCooldown = 1f;     
    public Transform attackPos;           
    public LayerMask whatIsEnemies;       

    [Header("Audio")]
    public AudioClip punchSound;          
    private AudioSource audioSource;      

    [Header("Animation")]
    public Animator playerAnim;           

    private float timeSinceLastAttack;   
    private Player_Controller playerController;

    void Start()
    {
        playerController = GetComponentInParent<Player_Controller>();
        playerAnim = GetComponentInParent<Animator>(); // Use parent's Animator component

        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound

        timeSinceLastAttack = 0f;
    }

    void Update()
    {
        timeSinceLastAttack -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timeSinceLastAttack <= 0)
        {
            PerformAttack();
            timeSinceLastAttack = attackCooldown;
        }
    }

    private void PerformAttack()
    {
        // Trigger attack animation
        SetAttackAnimation();

        // Play attack sound
        PlayAttackSound();

        // Detect enemies in attack range
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        foreach (var enemy in enemiesToDamage)
        {
            Debug.Log($"Detected enemy: {enemy.name}");

            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                Vector2 knockbackDirection = (enemy.transform.position - attackPos.position).normalized;
                enemyScript.TakeDamage(damage, knockbackDirection);
            }
            else if (enemy.TryGetComponent(out ScriptedEnemyAI scriptedEnemy))
            {
                Vector2 knockbackDirection = (enemy.transform.position - attackPos.position).normalized;
                scriptedEnemy.TakeDamage(damage, knockbackDirection);
            }
        }
    }

    private void SetAttackAnimation()
    {
        if (playerAnim == null) return;

        // Get the last movement direction from the player controller
        float lastMoveX = playerController.GetLastMoveX();
        float lastMoveY = playerController.GetLastMoveY();

        // Trigger attack animation
        playerAnim.SetBool("isSpace", true);
        playerAnim.SetFloat("moveX", lastMoveX);
        playerAnim.SetFloat("moveY", lastMoveY);

        // Reset the attack animation after a short delay
        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Adjust timing to match the animation
        playerAnim.SetBool("isSpace", false);
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && punchSound != null)
        {
            audioSource.loop = false;
            audioSource.priority = 0; // High priority
            audioSource.ignoreListenerPause = true;
            audioSource.PlayOneShot(punchSound);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }
    }
}
