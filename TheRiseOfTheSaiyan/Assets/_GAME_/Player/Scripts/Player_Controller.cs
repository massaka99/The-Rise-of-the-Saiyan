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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0) input.y = 0;

        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);

        bool isMoving = input != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            Vector2 targetPos = rb.position + input * moveSpeed * Time.deltaTime;

            if (IsWalkable(targetPos))
            {
                rb.MovePosition(targetPos);  
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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












//#region Enums
//private enum Directions{UP, DOWN, LEFT, RIGHT}
//#endregion

//#region Editor Data

//[Header("Movement Attributes")]
//[SerializeField] float _moveSpeed = 50f;

//[Header("Dependencies")]
//[SerializeField] Rigidbody2D _rb;
//[SerializeField] Animator _animator;
//[SerializeField] SpriteRenderer _spriteRenderer;
//#endregion

//#region Internal Data
//private Vector2 _moveDir = Vector2.zero;
//private Directions _facingDirections = Directions.RIGHT;
//#endregion

//#region Tick

//private void Update()
//{
//    GatherInput();
//    CalculateFacingDirection();
//    UpdateAnimations();
//}

//private void FixedUpdate()
//{
//    MovementUpdate();
//}

//#endregion

//#region Input Logic

//private void GatherInput()
//{
//    _moveDir.x = Input.GetAxisRaw("Horizontal");
//    _moveDir.y = Input.GetAxisRaw("Vertical");
//}

//#region Movement Logic

//private void MovementUpdate()
//{
//    _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
//}

//#endregion

//#region Animations Logic

//private void CalculateFacingDirection()
//{
//    if (_moveDir.x != 0)
//    {
//        _animator.SetBool("IsMovingSide", true);
//        if (_moveDir.x > 0) // move Right
//        {
//            _facingDirections = Directions.RIGHT;
//        }
//        else if (_moveDir.x < 0) // move Left
//        {
//            _facingDirections = Directions.LEFT;
//        }
//    }

//    if (_moveDir.y != 0)
//    {
//        _animator.SetBool("IsMovingUp", true);
//        if (_moveDir.y > 0) // move UP
//        {

//            _facingDirections = Directions.UP;
//        }
//        else if (_moveDir.y < 0) // move DOWN
//        {
//            _facingDirections = Directions.DOWN;
//        }
//    }
//}

//private void UpdateAnimations()
//{
//    if (_facingDirections == Directions.LEFT)
//    {
//        _spriteRenderer.flipX = true; 
//    }
//    else if (_facingDirections == Directions.RIGHT)
//    {
//        _spriteRenderer.flipX = false;
//    }
//}

//#endregion

//#endregion
