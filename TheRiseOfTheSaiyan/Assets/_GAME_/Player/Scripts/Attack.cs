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

    void Start()
    {
        // Search for the Player_Controller in the same GameObject or parent
        playerController = GetComponentInParent<Player_Controller>();

        // Check if playerController was found, to avoid NullReferenceException
        if (playerController == null)
        {
            Debug.LogError("Player_Controller component not found. Please ensure it's attached to the player GameObject or its parent.");
        }
    }


    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Trigger the attack animation and set the facing direction based on last movement
                playerAnim.SetBool("isSpace", true);

                // Get the last facing direction from Player_Controller
                float lastMoveX = playerController.GetLastMoveX();
                float lastMoveY = playerController.GetLastMoveY();

                playerAnim.SetFloat("moveX", lastMoveX);
                playerAnim.SetFloat("moveY", lastMoveY);

                // Deal damage to enemies within range
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }

                // Reset the attack cooldown
                timeBtwAttack = startBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        // Reset isSpace to prevent continuous triggering
        if (timeBtwAttack <= startBtwAttack - 0.1f)
        {
            playerAnim.SetBool("isSpace", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
