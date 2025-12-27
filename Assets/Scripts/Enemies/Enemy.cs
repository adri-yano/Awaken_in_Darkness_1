using UnityEngine;

/// <summary>
/// Alien enemy that chases the player, jumps over obstacles,
/// detects gaps, and damages the player on contact.
/// </summary>
public class AlienEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float jumpForce = 8f;
    public float obstacleCheckDistance = 1f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    [Header("Damage")]
    public int contactDamage = 20;
    public float contactCooldown = 0.5f;

    private Transform _player;
    private Rigidbody2D _rb;
    private bool _isGrounded;
    private float _lastHitTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        // Ground Check
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Direction to player
        float dir = Mathf.Sign(_player.position.x - transform.position.x);

        // Move
        Vector2 vel = _rb.linearVelocity;
        vel.x = dir * moveSpeed;
        _rb.linearVelocity = vel;

        // Jump logic
        if (ShouldJump(dir))
            Jump();

        // Face direction
        FaceDirection(dir);
    }

    private bool ShouldJump(float dir)
    {
        Vector2 feet = (Vector2)transform.position + Vector2.down * 0.1f;
        Vector2 front = (Vector2)transform.position + Vector2.right * dir * 0.6f;

        // Ground check
        _isGrounded = Physics2D.Raycast(feet, Vector2.down, groundCheckDistance, groundLayer);

        // Obstacle in front
        bool obstacleAhead = Physics2D.Raycast(front, Vector2.right * dir, obstacleCheckDistance, obstacleLayer);

        // Gap check
        bool gapAhead = !Physics2D.Raycast(front, Vector2.down, groundCheckDistance * 2, groundLayer);

        Debug.DrawRay(front, Vector2.right * dir * obstacleCheckDistance, Color.red);
        Debug.DrawRay(front, Vector2.down * (groundCheckDistance * 2), Color.yellow);

        return _isGrounded && (obstacleAhead || gapAhead);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private void FaceDirection(float dir)
    {
        if (Mathf.Approximately(dir, 0)) return;

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * Mathf.Sign(dir);
        transform.localScale = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.collider);
    }

    private void TryDamagePlayer(Collider2D collider)
    {
        if (Time.time - _lastHitTime < contactCooldown)
            return;

        PlayerMovement player = collider.GetComponent<PlayerMovement>();
        if (player != null && player.HasShield())
            return;

        PlayerHealth health = collider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            _lastHitTime = Time.time;
            health.ApplyDamage(contactDamage);
        }
    }
}