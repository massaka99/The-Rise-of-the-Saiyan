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
        playerController = GetComponentInParent<Player_Controller>();
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && timeBtwAttack <= 0)
        {
            playerAnim.SetBool("isSpace", true);

            float lastMoveX = playerController.GetLastMoveX();
            float lastMoveY = playerController.GetLastMoveY();

            playerAnim.SetFloat("moveX", lastMoveX);
            playerAnim.SetFloat("moveY", lastMoveY);

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
            }

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
