using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startBtwAttack;

    public float attackRange;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator playerAnim;
    public int damage;

    private Player_Controller playerController;

    public AudioClip punchSound;
    private AudioSource audioSource;

    void Start()
    {
        playerController = GetComponentInParent<Player_Controller>();
        playerAnim = GetComponent<Animator>();

        // Ensure an AudioSource is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && timeBtwAttack <= 0)
        {
            // Set animation trigger
            playerAnim.SetBool("isSpace", true);

            // Get last movement direction (even if standing still)
            float lastMoveX = playerController.GetLastMoveX();
            float lastMoveY = playerController.GetLastMoveY();

            playerAnim.SetFloat("moveX", lastMoveX);
            playerAnim.SetFloat("moveY", lastMoveY);

            // Always play the punch sound if available
            if (punchSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(punchSound);
            }

            // Perform the attack logic
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<Enemy>()?.TakeDamage(damage);
            }

            // Reset attack cooldown
            timeBtwAttack = startBtwAttack;
        }
        else
        {
            playerAnim.SetBool("isSpace", false);
        }

        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
