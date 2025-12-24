using UnityEngine;

public class HazardBlock : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth health = collision.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.ApplyDamage(damage);
        }
    }
}
