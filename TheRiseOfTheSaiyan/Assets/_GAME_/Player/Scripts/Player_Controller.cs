using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }

        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
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
}
