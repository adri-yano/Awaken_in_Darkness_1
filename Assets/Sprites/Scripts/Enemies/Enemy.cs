using UnityEngine;

/// <summary>
/// Simple chaser AI that runs toward the player, jumps over low obstacles,
/// and damages the player on contact. Works in 2D side scrollers.
/// </summary>
public class AlienEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float obstacleCheckDistance = 0.6f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    [Header("Damage")]
    public int contactDamage = 1;
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
        {
            _player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (_player == null) return;

        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        float dir = Mathf.Sign(_player.position.x - transform.position.x);
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = dir * moveSpeed;
        _rb.linearVelocity = velocity;

        if (ShouldJump(dir))
        {
            Jump();
        }

        FaceDirection(dir);
    }

    private bool ShouldJump(float dir)
    {
        Vector2 origin = (Vector2)transform.position + Vector2.right * dir * 0.25f;
        bool obstacleAhead = Physics2D.Raycast(origin, Vector2.right * dir, obstacleCheckDistance, obstacleLayer);
        bool gapAhead = !Physics2D.Raycast(origin + Vector2.right * dir * 0.3f, Vector2.down, groundCheckDistance * 2f, groundLayer);

        return _isGrounded && (obstacleAhead || gapAhead);
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private void FaceDirection(float dir)
    {
        if (Mathf.Approximately(dir, 0f)) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
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
        if (Time.time - _lastHitTime < contactCooldown) return;

        PlayerHealth health = collider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            _lastHitTime = Time.time;
            health.ApplyDamage(contactDamage);
        }
    }
}
