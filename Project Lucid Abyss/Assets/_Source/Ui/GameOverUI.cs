using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
    }

    public void ShowGameOver(string message)
    {
        gameOverPanel.SetActive(true);
        messageText.text = message;
        Time.timeScale = 0f;
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}