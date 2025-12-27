using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedMultiplier = 1.6f;
    public float duration = 6f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement pm = collision.GetComponent<PlayerMovement>();

        if (pm != null)
        {
            pm.StartCoroutine(pm.TemporarySpeed(speedMultiplier, duration));
            Destroy(gameObject);
        }
    }
}