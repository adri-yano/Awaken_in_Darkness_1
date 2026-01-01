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
    public float slideSpeedMultiplier = 1.2f;
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

    //---------------- SLIDE ----------------//
    void HandleSlide()
    {
        bool slideKey = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        if (slideKey && !isSliding)
            StartSlide();

        if (!slideKey && isSliding)
            StopSlide();

        // While sliding, tilt left/right if moving
        if (isSliding)
        {
            float move = Input.GetAxisRaw("Horizontal");

            if (move > 0)
                transform.rotation = Quaternion.Euler(0, 0, -70);   // MORE tilt
            else if (move < 0)
                transform.rotation = Quaternion.Euler(0, 0, 70);    // MORE tilt
            else
                transform.rotation = Quaternion.Euler(0, 0, -60);   // crouch tilt
        }
    }

    void StartSlide()
    {
        if (!isGrounded) return;

        isSliding = true;

        // shrink collider
        col.size = new Vector2(col.size.x, 0.35f);   // even smaller
        col.offset = new Vector2(col.offset.x, -0.5f);
    }

    void StopSlide()
    {
        isSliding = false;

        transform.rotation = Quaternion.identity;

        col.size = new Vector2(col.size.x, originalColliderHeight);
        col.offset = originalColliderOffset;
    }
    //----------------------------------------//

    void HandleFlip()
    {
        if (isSliding) return;

        float move = Input.GetAxisRaw("Horizontal");
        if (move == 0) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (move > 0 ? 1 : -1);
        transform.localScale = scale;
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