using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image deathImage;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float soundDelay = 1f;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float flickerDuration = 2f;
    [SerializeField] private AnimationCurve flickerCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    private CanvasGroup gameOverCanvasGroup;
    private LevelEndTrigger _levelEndTrigger;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
        messageText.gameObject.SetActive(false);
        messageText.text = "";
        restartButton.gameObject.SetActive(false);
        gameOverCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (gameOverCanvasGroup == null)
        {
            gameOverCanvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        gameOverCanvasGroup.alpha = 0f;
        if (deathImage != null) deathImage.gameObject.SetActive(false);
    }
    public void Constructor(LevelEndTrigger levelEndTrigger )
    {
        _levelEndTrigger = levelEndTrigger;
    }
    public void ShowGameOver(string message)
    {
        continueButton.gameObject.SetActive(_levelEndTrigger.ispass);
        StartCoroutine(ShowGameOverCoroutine(message));
    }

    private IEnumerator ShowGameOverCoroutine(string message)
    {
        gameOverPanel.SetActive(true);

        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(soundDelay);
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        restartButton.gameObject.SetActive(true);

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (deathImage != null)
        {
            deathImage.gameObject.SetActive(true);
            StartCoroutine(FlickerDeathImage());
        }

        Time.timeScale = 0f;
    }

    private IEnumerator FlickerDeathImage()
    {
        float timer = 0f;
        Color originalColor = deathImage.color;

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            float normalizedTime = timer % flickerDuration / flickerDuration;

            float alpha = flickerCurve.Evaluate(normalizedTime);
            deathImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }
    }

    private void RestartLevel()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    private void Continue()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
    }
}