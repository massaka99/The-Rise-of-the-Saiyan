using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    public float moveSpeed;
    public float runMultiplier = 2f;
    public LayerMask interactableLayer;
     
    private Vector2 input;
    private Animator animator;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;  

    AudioManager audioManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void HandleUpdate()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;

        bool isMoving = input != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        float currentSpeed = moveSpeed;

        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= runMultiplier; 
        }

        if (isMoving)
        {
            Vector2 targetPos = rb.position + input * currentSpeed * Time.deltaTime;

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

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }


        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, groundLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }
}
