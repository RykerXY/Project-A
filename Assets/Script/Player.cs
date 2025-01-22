using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
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
    [Header("Ground Collider Layer")]
    public LayerMask collisionLayer;
    [Header("Audio Source")]
    public CharacterAudio characterAudio;

    private bool isGrounded;
    private bool isJumping;
    private bool canDoubleJump;
    private bool isKnockback;
    private bool isStunned = false;
    private float highestPoint;
    private bool isChargJumpSound = false;

    private float doubleJumpTime;
    [SerializeField]private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        highestPoint = transform.position.y;
        characterAudio.playOnLoad();
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

            //หากตัวละครเดิน
            if(moveInput >= 0.1f || moveInput <= -0.1f) 
            {
                if(!characterAudio.audioSource.isPlaying)
                {
                Debug.Log(moveInput);
                characterAudio.PlayWalkSound();
                }
            }
            //หากตัวละครไม่เดิน
            else if(moveInput >= -0.1f || moveInput <= 0.1f && characterAudio.isPlaying()) 
            {
                if(characterAudio.getTime() >= characterAudio.getLength())
                {
                    Debug.Log("Audio Stop!");
                    characterAudio.StopAudio();
                }
            }
            if(moveInput == 0.000)
            {
                animator.SetBool("isMoving", false);
            }
            if(moveInput != 0)
            {
                animator.SetBool("isMoving", true);
            }
            if (Input.GetKey(KeyCode.D)) // เดินไปทางขวา
            {
                spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.A)) // เดินไปทางซ้าย
            {
                spriteRenderer.flipX = true;
            }

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
            animator.SetBool("isChargeJump", true);
            if(!isChargJumpSound)
            {
                Debug.Log(isChargJumpSound);
                characterAudio.PlayChargeJumpSound();
                isChargJumpSound = true;
            }
            else if(isChargJumpSound && !characterAudio.isPlaying())
            {
                isChargJumpSound = false;
            }
            
            if (jumpForce < maxJumpForce)
            {
                jumpForce += chargeSpeed * Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            animator.SetBool("isChargeJump", false);
            animator.SetBool("isJumping", true);
            characterAudio.PlayJumpSound();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }

        if (!isGrounded && canDoubleJump && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", true);
            characterAudio.PlayDoubleJumpSound();

            float moveDirection = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, doubleJumpForce);
            canDoubleJump = false;
            doubleJumpTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ชนกำแพง
        if (!isGrounded && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector2 contactPoint = collision.contacts[0].normal;

            if (Mathf.Abs(contactPoint.x) > Mathf.Abs(contactPoint.y))
            {
                animator.SetBool("isHit", true);
                StartCoroutine(ApplyKnockback(contactPoint));
            }
        }
        // ตกจากที่สูง
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Vector2 contactPoint = collision.contacts[0].normal;
            if (Mathf.Abs(contactPoint.x) < Mathf.Abs(contactPoint.y))
            {
                characterAudio.PlayLandingSound();
                animator.SetBool("isJumping", false);
                animator.SetBool("isHit", false);
                animator.SetBool("isDoubleJumping", false);
            }
            
            float fallDistance = highestPoint - transform.position.y;

            if (fallDistance > fallHeightThreshold)
            {
                StartCoroutine(StunPlayer());
            }

            highestPoint = transform.position.y;
        }
    }

    IEnumerator ApplyKnockback(Vector2 contactPoint)
    {
        //Debug.Log($"Contact Point: {contactPoint}, Knockback Force: {knockbackForce}");
        characterAudio.PlayBounceSound();

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
        characterAudio.PlayDazzlingSound();

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
