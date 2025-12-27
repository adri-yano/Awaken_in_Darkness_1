using UnityEngine;

public class InvisibilityPowerUp : MonoBehaviour
{
    public float duration = 6f;
    public int enemyLayer = 8;   // set to your Enemy layer index

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement p = collision.GetComponent<PlayerMovement>();

        if (p != null)
        {
            p.StartCoroutine(p.Invisibility(duration, enemyLayer));
            Destroy(gameObject);
        }
    }
}
