using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public float coyoteTime = 0.1f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastGroundedTime;
    private bool doubleJumpUsed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleFlip();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            doubleJumpUsed = false;
        }

        bool wantJump = Input.GetKeyDown(KeyCode.Space);
        bool canCoyoteJump = Time.time - lastGroundedTime <= coyoteTime;

        if (wantJump)
        {
            if (isGrounded || canCoyoteJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (!doubleJumpUsed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                doubleJumpUsed = true; // mark double jump as used
            }
        }
    }

    void HandleFlip()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
