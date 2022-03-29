using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D rgb;
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if(!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

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
        }
    }
}
