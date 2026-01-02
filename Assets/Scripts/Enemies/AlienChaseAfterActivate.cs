using UnityEngine;

public class AlienChaseAfterActivate : MonoBehaviour
{
    public float speed = 3f;
    public float damage = 10f;

    private Transform player;
    private bool isActive = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Enemy OFF at start
        enabled = false;
    }

    private void OnEnable()
    {
        isActive = true;
    }

    private void FixedUpdate()
    {
        if (!isActive || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth health = collision.collider.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Debug.Log("Player Damaged by Alien!");
        }
    }
}