using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float coyoteTime = 0.1f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool doubleJumpUsed;
    private float lastGroundedTime;
    private PlayerPowerUps _powerUps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _powerUps = GetComponent<PlayerPowerUps>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            doubleJumpUsed = false;
            lastGroundedTime = Time.time;
        }

        float speedMultiplier = _powerUps != null ? _powerUps.CurrentSpeedMultiplier : 1f;

        // Movement
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed * speedMultiplier, rb.linearVelocity.y);

        // Jump
        bool wantJump = Input.GetKeyDown(KeyCode.Space);
        bool canCoyoteJump = Time.time - lastGroundedTime <= coyoteTime;
        bool hasDoubleJump = _powerUps != null && _powerUps.HasDoubleJump;

        if (wantJump && (isGrounded || canCoyoteJump))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpUsed = false;
        }
        else if (wantJump && hasDoubleJump && !doubleJumpUsed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            doubleJumpUsed = true;
        }
    }
}
