using UnityEngine;
using static PlayerPowerUps;

/// <summary>
/// Attach to power-up pickup objects; grants the configured power to the player.
/// </summary>
public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType type = PowerUpType.Speed;
    public float duration = 8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPowerUps power = other.GetComponent<PlayerPowerUps>();
        if (power != null)
        {
            power.Activate(type, duration);
        }

        Destroy(gameObject);
    }
}

