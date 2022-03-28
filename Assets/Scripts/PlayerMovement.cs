using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10;
    Vector2 moveInput;
    Rigidbody2D rgb;

    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
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

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, rgb.velocity.y);
        rgb.velocity = playerVelocity;
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
