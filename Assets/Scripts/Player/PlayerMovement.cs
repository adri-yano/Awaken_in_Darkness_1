using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public float coyoteTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Sliding")]
    public float slideSpeedMultiplier = 1.4f;
    public float slideColliderHeight = 0.5f;
    private bool isSliding;

    // Powerups
    private bool doubleJumpEnabled = false;
    private bool doubleJumpUsed = false;
    private bool shieldActive = false;
    private bool invisible = false;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private bool isGrounded;
    private float lastGroundedTime;

    private float originalColliderHeight;
    private Vector2 originalColliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        originalColliderHeight = col.size.y;
        originalColliderOffset = col.offset;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFlip();
        HandleSlide();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        float speed = isSliding ? moveSpeed * slideSpeedMultiplier : moveSpeed;

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            doubleJumpUsed = false;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        bool coyote = Time.time - lastGroundedTime <= coyoteTime;

        if (isGrounded || coyote)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            return;
        }

        if (doubleJumpEnabled && !doubleJumpUsed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpUsed = true;
        }
    }

    void HandleSlide()
    {
        bool slideKey = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (slideKey && isGrounded && !isSliding)
            StartSlide();

        if (!slideKey && isSliding)
            StopSlide();
    }

    void StartSlide()
    {
        isSliding = true;

        // NO ROTATION = no flicker
        col.size = new Vector2(col.size.x, slideColliderHeight);
        col.offset = new Vector2(col.offset.x, -0.2f);
    }

    void StopSlide()
    {
        isSliding = false;

        col.size = new Vector2(col.size.x, originalColliderHeight);
        col.offset = originalColliderOffset;
    }

    void HandleFlip()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public bool HasShield() => shieldActive;
    public bool IsSliding() => isSliding;

    // ---- POWER UPS ----
    public IEnumerator TemporarySpeed(float m, float d)
    {
        float original = moveSpeed;
        moveSpeed *= m;
        yield return new WaitForSeconds(d);
        moveSpeed = original;
    }

    public IEnumerator EnableDoubleJump(float d)
    {
        doubleJumpEnabled = true;
        yield return new WaitForSeconds(d);
        doubleJumpEnabled = false;
    }

    public IEnumerator Shield(float d)
    {
        shieldActive = true;
        yield return new WaitForSeconds(d);
        shieldActive = false;
    }

    public IEnumerator Invisibility(float duration, int enemyLayer)
    {
        invisible = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);
        yield return new WaitForSeconds(duration);
        invisible = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
    }
}