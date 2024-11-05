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
    public Animator playerAnim; // Make sure this Animator is linked to the Player's Animator Controller
    public int damage;

    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Set the attack trigger and direction for the player's animation
                playerAnim.SetBool("isSpace", true);

                // Set the direction based on movement or player facing direction
                playerAnim.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
                playerAnim.SetFloat("moveY", Input.GetAxisRaw("Vertical"));

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
        if (timeBtwAttack <= startBtwAttack - 0.1f) // Adjust time as needed for smooth transitions
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
