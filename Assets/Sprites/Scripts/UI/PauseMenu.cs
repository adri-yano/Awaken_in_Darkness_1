using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Toggles pause state and shows/hides the pause panel.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public UnityEvent onPaused;
    public UnityEvent onUnpaused;

    private bool _isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        SetPaused(!_isPaused);
    }

    public void Resume()
    {
        SetPaused(false);
    }

    private void SetPaused(bool value)
    {
        if (_isPaused == value) return;

        _isPaused = value;
        Time.timeScale = _isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(_isPaused);
        }

        if (_isPaused)
            onPaused?.Invoke();
        else
            onUnpaused?.Invoke();
    }
}

