using System;
using UnityEngine;

/// <summary>
/// Calculates progress from a start to end point for UI display.
/// </summary>
public class LevelProgressTracker : MonoBehaviour
{
    public Transform player;
    public Transform start;
    public Transform end;

    public event Action<float> OnProgressChanged;

    private float _distance;

    private void Start()
    {
        if (start != null && end != null)
        {
            _distance = Mathf.Abs(end.position.x - start.position.x);
        }
    }

    private void Update()
    {
        if (player == null || start == null || end == null || _distance <= 0f) return;

        float travelled = Mathf.Abs(player.position.x - start.position.x);
        float percent = Mathf.Clamp01(travelled / _distance);
        OnProgressChanged?.Invoke(percent);
    }
}

