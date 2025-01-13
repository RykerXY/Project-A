using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float bounceForce = 5f;

    [Header("Jump Settings")]
    public float maxJumpForce = 20f;  // Maximum jump strength
    public float minJumpForce = 5f;   // Minimum jump strength
    public float chargeTime = 1f;     // Time to reach max charge

    [Header("Ground Check Settings")]
    public Transform groundCheck;     // Transform for the ground check position
    public float groundCheckRadius = 0.1f; // Radius for the ground check
    public LayerMask groundLayer;     // Layer to identify as ground

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isCharging = false;
    private float currentJumpForce = 0f;
    private float chargeTimer = 0f;

    private bool canDoubleJump = false;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check using raycast
        CheckGround();

        // Handle movement (only if not charging a jump)
        if (!isCharging && canMove)
        {
            HandleMovement();
        }

        // Handle jump charging
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StartChargingJump();
        }

        if (Input.GetButton("Jump") && isCharging)
        {
            ChargeJump();
        }

        if (Input.GetButtonUp("Jump") && isCharging)
        {
            PerformJump();
        }

        // Double jump when not grounded
        if (Input.GetButtonDown("Jump") && canDoubleJump && !isGrounded)
        {
            DoubleJump();
        }
        if(isGrounded)
        {
            canMove = true;
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip the sprite based on the movement direction
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Facing right
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // Facing left
        }
    }

    private void StartChargingJump()
    {
        isCharging = true;
        chargeTimer = 0f;
        currentJumpForce = minJumpForce;

        // Stop player movement when charging starts
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void ChargeJump()
    {
        chargeTimer += Time.deltaTime;
        currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeTimer / chargeTime);
        currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);
    }

    private void PerformJump()
    {
        isCharging = false;

        // Apply the jump force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);
        isGrounded = false;
        canDoubleJump = true;
    }

    private void DoubleJump()
    {
        isCharging = false;

        // Apply the jump force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, minJumpForce);
        isGrounded = false;
        canDoubleJump = false;
    }

    private void CheckGround()
    {
        // Perform a circle cast (raycast with radius) to check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        // Draw the ground check circle in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision with: " + collision.gameObject.name);
        
        if(!isGrounded)
        {
            canMove = false;
            // Calculate bounce direction
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;

            // Apply bounce force
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        }
    }
}
