using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    public AlienEnemy enemy;

    void Start()
    {
        if (enemy != null)
            enemy.enabled = false;   // enemy OFF at start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemy) return;

        if (collision.CompareTag("Player"))
        {
            enemy.enabled = true;

            // Wake Rigidbody so it doesn't freeze
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.WakeUp();

            Debug.Log("Enemy Activated!");
        }
    }
}