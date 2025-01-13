using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float jumpForce = 10f; // Maximum jump force
    public float jumpHorizontalForce = 5f; // Horizontal jump force
    public float gravity = -9.8f; // Gravity strength
    public LayerMask groundLayer; // Ground layer

    private float currentJumpForce = 0f; // Current jump force applied
    private bool isGrounded; // Is the player grounded?
    private bool isChargingJump; // Is the player currently charging a jump?

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Only allow movement if grounded and not charging jump
        if (isGrounded && !isChargingJump)
        {
            MovePlayer();
        }

        // Jumping Mechanism
        JumpMechanism();
    }

    private void MovePlayer()
    {
        // Only allow horizontal movement if grounded
        if (isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // Left/Right movement

            // Move the player horizontally only if grounded and not charging jump
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void JumpMechanism()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.Space)) // Hold to charge jump
            {
                if (!isChargingJump)
                    isChargingJump = true;

                // Charge jump force
                if (currentJumpForce < jumpForce)
                    currentJumpForce += Time.deltaTime * jumpForce;
            }

            if (Input.GetKeyUp(KeyCode.Space)) // Release to jump
            {
                // Apply both horizontal and vertical jump force
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + jumpHorizontalForce * (Input.GetAxis("Horizontal")), currentJumpForce);
                currentJumpForce = 0f;
                isChargingJump = false;
            }
        }
        else
        {
            // Apply gravity when in the air
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + gravity * Time.deltaTime);
        }
    }

    // Called when the player collides with something
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            // Player is grounded
            isGrounded = true;
        }
    }

    // Called when the player stops colliding with something
    void OnCollisionExit2D(Collision2D collision)
    {
        // If the player is no longer colliding with the ground, they are in the air
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
