using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Transform respawnPoint; // assign in inspector

    // Event for health changes (current and max health)
    public event Action<int, int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify UI at start
    }

    public void ApplyDamage(int amount)
    {
        currentHealth -= amount;

        // Notify UI
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}