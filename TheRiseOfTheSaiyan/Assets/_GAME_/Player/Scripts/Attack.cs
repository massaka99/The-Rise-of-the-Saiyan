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
        timeSinceMeleeAttack -= Time.deltaTime;
        timeSinceSpecialAttack -= Time.deltaTime;

        // Melee attack
        if (Input.GetKeyDown(KeyCode.Space) && timeSinceMeleeAttack <= 0)
        {
            PerformMeleeAttack();
            timeSinceMeleeAttack = meleeAttackCooldown;
        }

        // Special attack (Kamehameha)
        if (Input.GetKeyDown(KeyCode.K) && timeSinceSpecialAttack <= 0)
        {
            PerformKamehameha();
            timeSinceSpecialAttack = kamehamehaCooldown;
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
        DealDamage(kamehamehaPos.position, kamehamehaRange, kamehamehaDamage);
    }

    private void DealDamage(Vector2 position, float range, int damage)
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(position, range, whatIsEnemies);

        foreach (var enemy in enemiesToDamage)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            
            if (enemy.TryGetComponent(out Enemy enemyScript))
            {
                enemyScript.TakeDamage(damage, knockbackDirection);
            }
            else if (enemy.TryGetComponent(out ScriptedEnemyAI scriptedEnemy))
            {
                scriptedEnemy.TakeDamage(damage, knockbackDirection);
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

        StartCoroutine(ResetAttackAnimation(triggerName));
    }

    private IEnumerator ResetAttackAnimation(string triggerName)
    {
        yield return new WaitForSeconds(triggerName == "isKamehameha" ? 0.5f : 0.1f);
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
