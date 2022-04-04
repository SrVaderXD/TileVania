using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D rgb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    float gravityBeforeClimbing;

    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityBeforeClimbing = rgb.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

        if(value.isPressed)
        {
            rgb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, rgb.velocity.y);
        rgb.velocity = playerVelocity;

        bool playerMoving = Mathf.Abs(rgb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerMoving);
    }

    void FlipSprite()
    {
        bool playerMoving = Mathf.Abs(rgb.velocity.x) > Mathf.Epsilon;

        if(playerMoving)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rgb.velocity.x), 1f);
            animator.SetBool("isClimbing", false);
        }
    }

    void ClimbLadder()
    {
        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rgb.gravityScale = gravityBeforeClimbing;
            return;
        }

        Vector2 climbVelocity = new Vector2 (rgb.velocity.x, moveInput.y * climbSpeed);
        rgb.velocity = climbVelocity;
        rgb.gravityScale = 0;

        bool playerClimbing = Mathf.Abs(rgb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerClimbing);
    }
}
