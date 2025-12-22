using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tracks player health, manages invincibility frames, and signals UI updates.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibleDuration = 1.2f;
    public float deathResetDelay = 1.5f;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDied;

    private int _currentHealth;
    private bool _invincible;
    private float _invincibleUntil;
    private PlayerPowerUps _powerUps;

    private void Awake()
    {
        _currentHealth = maxHealth;
        _powerUps = GetComponent<PlayerPowerUps>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    private void Update()
    {
        if (_invincible && Time.time > _invincibleUntil)
        {
            _invincible = false;
        }
    }

    public void ApplyDamage(int amount)
    {
        if (amount <= 0) return;
        if (_invincible) return;

        if (_powerUps != null && _powerUps.TryConsumeShield())
        {
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            return;
        }

        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            _invincible = true;
            _invincibleUntil = Time.time + invincibleDuration;
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || _currentHealth <= 0) return;
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();
        Invoke(nameof(ReloadScene), deathResetDelay);
    }

    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

