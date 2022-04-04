using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRGB;
    void Start()
    {
        enemyRGB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyRGB.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(- (Mathf.Sign(enemyRGB.velocity.x)), 1f);
    }
}
