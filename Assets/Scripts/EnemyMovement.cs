using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    BoxCollider2D enemyGroundDetector;

    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        enemyGroundDetector = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        flipSprite();
    }
    void flipSprite()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody.velocity.x), 1f);
    }
}
