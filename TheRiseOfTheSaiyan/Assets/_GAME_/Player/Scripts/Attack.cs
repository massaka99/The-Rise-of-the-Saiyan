using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public float meleeAttackRange = 1f;         
    public int meleeDamage = 10;               
    public float meleeAttackCooldown = 1f;     
    public Transform meleeAttackPos;           

    [Header("Special Attack Settings")]
    public float kamehamehaRange = 5f;
    public int kamehamehaDamage = 50;
    public float kamehamehaCooldown = 3f;
    public Transform kamehamehaPos;

    [Header("General Settings")]
    public LayerMask whatIsEnemies;       

    [Header("Audio")]
    public AudioClip punchSound;
    public AudioClip kamehamehaSound;          
    private AudioSource audioSource;      

    [Header("Animation")]
    public Animator playerAnim;           

    private float timeSinceMeleeAttack;
    private float timeSinceSpecialAttack;   
    private Player_Controller playerController;

    void Start()
    {
        playerController = GetComponentInParent<Player_Controller>();
        playerAnim = GetComponentInParent<Animator>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        timeSinceMeleeAttack = 0f;
        timeSinceSpecialAttack = 0f;
    }

    void Update()
    {
        // Decrease the cooldown timers
        if (timeSinceMeleeAttack > 0)
            timeSinceMeleeAttack -= Time.deltaTime;

        if (timeSinceSpecialAttack > 0)
            timeSinceSpecialAttack -= Time.deltaTime;

        // Melee attack logic
        if (Input.GetKeyDown(KeyCode.Space) && timeSinceMeleeAttack <= 0)
        {
            PerformMeleeAttack();
            timeSinceMeleeAttack = meleeAttackCooldown; // Reset melee cooldown
        }

        // Kamehameha attack logic
        if (Input.GetKeyDown(KeyCode.K) && timeSinceSpecialAttack <= 0)
        {
            PerformKamehameha();
            timeSinceSpecialAttack = kamehamehaCooldown; // Reset special attack cooldown
        }
    }


    private void PerformMeleeAttack()
    {
        SetAttackAnimation("isSpace");
        PlaySound(punchSound);
        DealDamage(meleeAttackPos.position, meleeAttackRange, meleeDamage);
    }

    private void PerformKamehameha()
    {
        SetAttackAnimation("isKamehameha");
        PlaySound(kamehamehaSound);

        // Convert kamehamehaPos.position to a Vector2
        Vector2 attackPosition = (Vector2)kamehamehaPos.position;

        // Adjust attack position based on player's last movement direction
        attackPosition += new Vector2(playerController.GetLastMoveX(), playerController.GetLastMoveY()).normalized * kamehamehaRange;

        // Deal damage in the adjusted position
        DealDamage(attackPosition, kamehamehaRange, kamehamehaDamage);
    }



    private void DealDamage(Vector2 position, float range, int damage)
    {
        // Check if enemies are detected in the specified range
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(position, range, whatIsEnemies);

        if (enemiesToDamage.Length == 0)
        {
            Debug.Log("No enemies detected for Kamehameha attack.");
            return;
        }

        foreach (var enemy in enemiesToDamage)
        {
            Debug.Log($"Enemy detected: {enemy.name}");

            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;

            // Apply damage to the enemy
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                enemyScript.TakeDamage(damage, knockbackDirection);
            }
            else if (enemy.TryGetComponent(out ScriptedEnemyAI scriptedEnemy))
            {
                scriptedEnemy.TakeDamage(damage, knockbackDirection);
            }
            else
            {
                Debug.Log($"Enemy {enemy.name} does not have a damageable script attached.");
            }
        }
    }


    private void SetAttackAnimation(string triggerName)
    {
        if (playerAnim == null) return;

        float lastMoveX = playerController.GetLastMoveX();
        float lastMoveY = playerController.GetLastMoveY();

        playerAnim.SetBool(triggerName, true);
        playerAnim.SetFloat("moveX", lastMoveX);
        playerAnim.SetFloat("moveY", lastMoveY);

        // Extend the animation duration to match Kamehameha's full length
        float animationDuration = triggerName == "isKamehameha" ? 0.6f : 0.2f; // Adjust duration as per animation length
        StartCoroutine(ResetAttackAnimation(triggerName, animationDuration));
    }


    private IEnumerator ResetAttackAnimation(string triggerName, float duration)
    {
        yield return new WaitForSeconds(duration);
        playerAnim.SetBool(triggerName, false);
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleeAttackPos.position, meleeAttackRange);
        }
        if (kamehamehaPos != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(kamehamehaPos.position, kamehamehaRange);
        }
    }
}
