using UnityEngine;

public class TimedGround : MonoBehaviour
{
    public float disappearDelay = 1.5f;
    public float respawnTime = 3f;

    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke(nameof(Disappear), disappearDelay);
        }
    }

    void Disappear()
    {
        col.enabled = false;
        sr.enabled = false;

        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        col.enabled = true;
        sr.enabled = true;
    }
}
