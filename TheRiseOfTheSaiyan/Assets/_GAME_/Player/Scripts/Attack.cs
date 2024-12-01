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
        playerAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

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
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                // Calculate knockback direction
                Vector2 knockbackDirection = (enemy.transform.position - attackPos.position).normalized;

                // Apply damage and knockback
                enemyScript.TakeDamage(damage, knockbackDirection);
            }
        }
    }


    private void SetAttackAnimation()
    {
        if (playerAnim == null) return;

        float lastMoveX = playerController.GetLastMoveX();
        float lastMoveY = playerController.GetLastMoveY();

        playerAnim.SetBool("isSpace", true);
        playerAnim.SetFloat("moveX", lastMoveX);
        playerAnim.SetFloat("moveY", lastMoveY);

        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetBool("isSpace", false);
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && punchSound != null)
        {
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
