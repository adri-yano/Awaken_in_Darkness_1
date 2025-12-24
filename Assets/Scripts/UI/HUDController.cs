using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Drives HUD elements: health bar, progress bar, and power-up icons.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerPowerUps powerUps;
    public LevelProgressTracker progressTracker;

    [Header("UI")]
    public Slider healthBar;
    public Slider progressBar;
    public Image speedIcon;
    public Image shieldIcon;
    public Image doubleJumpIcon;
    public Image cloakIcon;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }

        if (powerUps != null)
        {
            powerUps.OnPowerUpStateChanged += HandlePowerStateChanged;
        }

        if (progressTracker != null)
        {
            progressTracker.OnProgressChanged += HandleProgressChanged;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }

        if (powerUps != null)
        {
            powerUps.OnPowerUpStateChanged -= HandlePowerStateChanged;
        }

        if (progressTracker != null)
        {
            progressTracker.OnProgressChanged -= HandleProgressChanged;
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (healthBar == null) return;
        healthBar.maxValue = max;
        healthBar.value = current;
    }

    private void HandleProgressChanged(float value)
    {
        if (progressBar == null) return;
        progressBar.normalizedValue = value;
    }

    private void HandlePowerStateChanged(PlayerPowerUps.PowerUpType type, bool active)
    {
        switch (type)
        {
            case PlayerPowerUps.PowerUpType.Speed:
                SetIcon(speedIcon, active);
                break;
            case PlayerPowerUps.PowerUpType.Shield:
                SetIcon(shieldIcon, active);
                break;
            case PlayerPowerUps.PowerUpType.DoubleJump:
                SetIcon(doubleJumpIcon, active);
                break;
            case PlayerPowerUps.PowerUpType.Invisibility:
                SetIcon(cloakIcon, active);
                break;
        }
    }

    private void SetIcon(Image icon, bool active)
    {
        if (icon == null) return;
        icon.enabled = active;
        Color c = icon.color;
        c.a = active ? 1f : 0.3f;
        icon.color = c;
    }
}

