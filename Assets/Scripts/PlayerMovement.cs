using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    Vector2 moveInput;
    Rigidbody2D rgb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    float gravityBeforeClimbing;
    bool isAlive = true;

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
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        PlayerDeath();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isAlive){return;}

        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

        if(value.isPressed)
        {
            rgb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive){return;}

        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnInteract(InputValue value)
    {
        if(!isAlive){return;}

        if(!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Interactables"))) {return;}

        if(FindObjectOfType<LevelExit>().nextLevel)
        {
            StartCoroutine(FindObjectOfType<LevelExit>().LoadNextLevel());
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

    void PlayerDeath()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rgb.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
