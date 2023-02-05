using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    [SerializeField] float VelocityMultiplier = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowSpawn;
    float DefaultPlayerGravity;
    Animator myAnimator;

    bool playerIsClimbing;
    //bool playerIsJumping;
    bool isAlive = true;
    [SerializeField] Vector2 deathKick = new Vector2(-20f, 20f);

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        DefaultPlayerGravity = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();

        Die();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
        {
            return; // if not touching ground and not touching climbables, do nothing
        }
        if(value.isPressed)
        {
            playerIsClimbing = false;
            //playerIsJumping = true;
            myRigidbody.velocity += new Vector2(0, jumpSpeed);
        }   
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;

        myAnimator.SetTrigger("Shooting");
    }
    void FireArrow()
    {
        Instantiate(arrow, arrowSpawn.position, transform.rotation);
    }

    void Run() 
    {
        if (!isAlive) return;
        Vector2 playerVelocity = new Vector2(moveInput.x * VelocityMultiplier, myRigidbody.velocity.y); // Y part is not 0 to avoid slowly falling
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; // Bool for animation
        myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; // Greater than basically zero

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    void ClimbLadder()
    {
        myAnimator.SetBool("IsClimbing", playerIsClimbing);
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            SetPlayerGravity(DefaultPlayerGravity);
            playerIsClimbing = false;
            myAnimator.speed = 1f;
            return;
        }
        // Touching a ladder, either climbing or not:
        Vector2 ClimbVelocity;
        SetPlayerGravity(0f);
        //bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        playerIsClimbing = true;
        if (moveInput.y != 0) // Actively climbing up or down
        {
            ClimbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = ClimbVelocity;
            myAnimator.speed = 1f;
        }
        else // Touching ladder but not climbing
        {
            //TODO check if playerIsJumping? allow jumping off ladder when stopped
            ClimbVelocity = new Vector2(myRigidbody.velocity.x, 0);
            myRigidbody.velocity = ClimbVelocity;
            myAnimator.speed = 0f;
        }
    }

    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            isAlive = false;
            myRigidbody.velocity += deathKick;
            Debug.Log("Player killed by enemy");
        }
        else if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            myRigidbody.velocity = new Vector2(0f, -2f);
            Debug.Log("Player killed by hazard");
        }
        if (!isAlive)
        {
            myAnimator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }

    void SetPlayerGravity(float GScale)
    {
        myRigidbody.gravityScale = GScale;
    }
}
