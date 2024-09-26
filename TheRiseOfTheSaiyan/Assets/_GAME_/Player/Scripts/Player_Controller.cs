using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 input;

    private Animator animator;

    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;


        bool isMoving = input != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            Vector2 targetPos = rb.position + input * moveSpeed * Time.deltaTime;

            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

            if (IsWalkable(targetPos))
            {
                rb.MovePosition(targetPos);
            }

            if (input.x != 0)
            {
                spriteRenderer.flipX = input.x < 0;
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetBool("isMoving", isMoving);
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, groundLayer) != null)
        {
            return false;
        }

        return true;
    }
}
