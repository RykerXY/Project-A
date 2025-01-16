using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxJumpForce = 15f;
    public float chargeSpeed = 5f;
    public float jumpForce = 0f;
    public float doubleJumpForce = 10f;
    public float doubleJumpCooldown = 0.2f;
    [Header("Stun Settings")]
    public float knockbackDuration = 0.5f;
    public float knockbackForce = 5f;
    public float stunDuration = 2f;
    public float fallHeightThreshold = 10f;
    private bool isStunned = false;
    private float highestPoint;
    [Header("Ground Collider Layer")]
    public LayerMask collisionLayer;

    private bool isGrounded;
    private bool isJumping;
    private bool canDoubleJump;
    private bool isKnockback;

    private float doubleJumpTime;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        highestPoint = transform.position.y;
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(capsuleCollider, LayerMask.GetMask("Ground"));
        
        if (!isKnockback)
        {
            if (isGrounded && !isJumping)
            {
                Move();
            }
            Jump();
        }
        if (!isStunned && rb.linearVelocity.y > 0)
        {
            highestPoint = Mathf.Max(highestPoint, transform.position.y);
        }
    }

    void Move()
    {
        if (!isKnockback && !isStunned)
        {
            float moveInput = Input.GetAxis("Horizontal");

            if (!isJumping)
            {
                Vector2 movement = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
                rb.linearVelocity = movement;
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpForce = 0f;
            canDoubleJump = true;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce += chargeSpeed * Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }

        if (!isGrounded && canDoubleJump && Input.GetButtonDown("Jump"))
        {
            float moveDirection = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, doubleJumpForce);
            canDoubleJump = false;
            doubleJumpTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            Vector2 contactPoint = collision.contacts[0].normal;

            if (Mathf.Abs(contactPoint.x) > Mathf.Abs(contactPoint.y))
            {
                StartCoroutine(ApplyKnockback(contactPoint));
            }
        }
        // ตรวจสอบการชนกับพื้น
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            float fallDistance = highestPoint - transform.position.y;

            if (fallDistance > fallHeightThreshold)
            {
                StartCoroutine(StunPlayer());
            }

            // รีเซ็ตจุดสูงสุดหลังจากชนพื้น
            highestPoint = transform.position.y;
        }
    }

    IEnumerator ApplyKnockback(Vector2 contactPoint)
    {
        Debug.Log($"Contact Point: {contactPoint}, Knockback Force: {knockbackForce}");

        isKnockback = true;
        if(contactPoint.x < 0)
        {
            Vector2 knockbackDirection = new Vector2(-1, 0);
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        if(contactPoint.x > 0)
        {
            Vector2 knockbackDirection = new Vector2(1, 0);
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }
    private IEnumerator StunPlayer()
    {
        if (!isStunned)
        {
            isStunned = true;
            Debug.Log("Player is stunned!");
            rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(stunDuration);

            isStunned = false;
            Debug.Log("Player is no longer stunned!");
        }
    }
}
