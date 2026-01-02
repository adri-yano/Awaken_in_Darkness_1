using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIController : MonoBehaviour
{
    [Header("Powerup Icons")]
    public Image speedIcon;
    public Image shieldIcon;
    public Image doubleJumpIcon;
    public Image cloakIcon;

    void Start()
    {
        // Hide all icons at start
        speedIcon.enabled = false;
        shieldIcon.enabled = false;
        doubleJumpIcon.enabled = false;
        cloakIcon.enabled = false;
    }

    public void ShowSpeed(bool show) => speedIcon.enabled = show;
    public void ShowShield(bool show) => shieldIcon.enabled = show;
    public void ShowDoubleJump(bool show) => doubleJumpIcon.enabled = show;
    public void ShowCloak(bool show) => cloakIcon.enabled = show;
}