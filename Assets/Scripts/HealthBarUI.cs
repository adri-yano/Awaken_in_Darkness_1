using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth player;
    public Slider slider;

    void Start()
    {
        slider.maxValue = player.maxHealth;
        slider.value = player.currentHealth;
    }

    void Update()
    {
        slider.value = player.currentHealth;
    }
}
