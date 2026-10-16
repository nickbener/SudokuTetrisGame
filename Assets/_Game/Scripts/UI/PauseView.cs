using UnityEngine;

public class PauseView : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameOverPanel;

    public void SetPauseState(bool isPaused)
    {
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(isPaused);
        }
    }

    public void ShowGameOver(bool show)
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(show);
    }
}