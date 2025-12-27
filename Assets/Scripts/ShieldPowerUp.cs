using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public float duration = 8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement p = collision.GetComponent<PlayerMovement>();

        if (p != null)
        {
            p.StartCoroutine(p.Shield(duration));
            Destroy(gameObject);
        }
    }
}