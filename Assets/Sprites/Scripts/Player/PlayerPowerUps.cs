using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles temporary power-ups and exposes state for UI and movement.
/// </summary>
public class PlayerPowerUps : MonoBehaviour
{
    public enum PowerUpType { Speed, Shield, DoubleJump, Invisibility }

    [Serializable]
    public class PowerUpConfig
    {
        public PowerUpType type;
        public float duration = 8f;
    }

    public float speedMultiplier = 1.4f;

    public event Action<PowerUpType, bool> OnPowerUpStateChanged;

    private readonly Dictionary<PowerUpType, float> _timers = new Dictionary<PowerUpType, float>();

    public bool HasDoubleJump => IsActive(PowerUpType.DoubleJump);
    public bool HasShield => IsActive(PowerUpType.Shield);
    public bool IsInvisible => IsActive(PowerUpType.Invisibility);
    public float CurrentSpeedMultiplier => IsActive(PowerUpType.Speed) ? speedMultiplier : 1f;

    private void Update()
    {
        if (_timers.Count == 0) return;

        List<PowerUpType> expired = null;
        foreach (var kvp in _timers)
        {
            if (Time.time > kvp.Value)
            {
                expired ??= new List<PowerUpType>();
                expired.Add(kvp.Key);
            }
        }

        if (expired == null) return;

        foreach (PowerUpType type in expired)
        {
            _timers.Remove(type);
            OnPowerUpStateChanged?.Invoke(type, false);
        }
    }

    public void Activate(PowerUpType type, float duration)
    {
        float until = Time.time + duration;
        _timers[type] = until;
        OnPowerUpStateChanged?.Invoke(type, true);
    }

    public bool IsActive(PowerUpType type)
    {
        return _timers.TryGetValue(type, out float until) && Time.time <= until;
    }

    public bool TryConsumeShield()
    {
        if (!IsActive(PowerUpType.Shield)) return false;
        _timers.Remove(PowerUpType.Shield);
        OnPowerUpStateChanged?.Invoke(PowerUpType.Shield, false);
        return true;
    }
}

