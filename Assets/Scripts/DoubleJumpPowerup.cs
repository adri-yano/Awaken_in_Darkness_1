using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement p = collision.GetComponent<PlayerMovement>();

        if (p != null)
        {
            p.StartCoroutine(p.EnableDoubleJump(duration));
            Destroy(gameObject);
        }
    }
}
