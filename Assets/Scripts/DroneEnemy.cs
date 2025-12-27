using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    [Header("Damage")]
    public int damage = 20;
    public float hitCooldown = 0.5f;

    private Vector3 startPos;
    private float lastHitTime;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2f);
        transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamagePlayer(collision);
    }

    void TryDamagePlayer(Collider2D collision)
    {
        if (Time.time - lastHitTime < hitCooldown)
            return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player == null) return;

        //  Player is sliding?
        if (player.IsSliding())
        {
            // No damage if sliding under correctly
            return;
        }

        PlayerHealth health = collision.GetComponent<PlayerHealth>();
        if (health != null)
        {
            lastHitTime = Time.time;
            health.ApplyDamage(damage);
        }
    }
}
