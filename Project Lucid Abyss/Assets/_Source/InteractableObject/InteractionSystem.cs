using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text interactionPromptText;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 1.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float promptHeight = 1.5f;
    [SerializeField] private float fadeSpeed = 5f;

    private InteractableObject currentNearbyObject;
    private bool hasNearbyObject = false;
    private CanvasGroup promptCanvasGroup;

    private void Awake()
    {
        // Инициализация UI
        interactionPanel.SetActive(false);

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            promptCanvasGroup = interactionPrompt.GetComponent<CanvasGroup>();
            if (promptCanvasGroup == null)
            {
                promptCanvasGroup = interactionPrompt.AddComponent<CanvasGroup>();
            }
            promptCanvasGroup.alpha = 0;
        }

        // Настройка кнопок
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void Update()
    {
        CheckNearbyObjects();
        UpdateInteractionPrompt();
    }

    private void CheckNearbyObjects()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);
        hasNearbyObject = false;
        currentNearbyObject = null;

        // Находим ближайший доступный объект
        foreach (var collider in hitColliders)
        {
            var obj = collider.GetComponent<InteractableObject>();
            if (obj != null && !obj.WasCollected)
            {
                hasNearbyObject = true;
                currentNearbyObject = obj;
                break; // Берем первый попавшийся объект
            }
        }
    }

    private void UpdateInteractionPrompt()
    {
        if (interactionPrompt == null) return;

        float targetAlpha = (hasNearbyObject && !interactionPanel.activeSelf) ? 1f : 0f;
        promptCanvasGroup.alpha = Mathf.Lerp(promptCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        if (promptCanvasGroup.alpha > 0.01f)
        {
            if (!interactionPrompt.activeSelf)
            {
                interactionPrompt.SetActive(true);
            }

            // Обновляем текст и позицию
            interactionPromptText.text = $"Нажмите {KeyCode.E} чтобы взаимодействовать";
            UpdatePromptPosition();
        }
        else if (interactionPrompt.activeSelf)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void UpdatePromptPosition()
    {
        if (Camera.main != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * promptHeight;
            interactionPrompt.transform.position = worldPosition;
        }
    }

    public void TryInteract()
    {
        if (hasNearbyObject && currentNearbyObject != null && !currentNearbyObject.WasCollected)
        {
            ShowInteractionDialog(currentNearbyObject);
        }
    }

    private void ShowInteractionDialog(InteractableObject obj)
    {
        currentNearbyObject = obj;
        questionText.text = $"Вы хотите взять этот предмет? (Аномалия: {(obj.IsAnomaly ? "Да" : "Нет")})";
        interactionPanel.SetActive(true);

        // Останавливаем время
        Time.timeScale = 0f;

        // Делаем кнопки снова интерактивными (на случай повторного открытия)
        yesButton.interactable = true;
        noButton.interactable = true;
    }

    private void OnYesClicked()
    {
        if (currentNearbyObject != null)
        {
            currentNearbyObject.Collect();
        }
        CloseInteractionDialog();
    }

    private void OnNoClicked()
    {
        CloseInteractionDialog();
    }

    private void CloseInteractionDialog()
    {
        interactionPanel.SetActive(false);
        // Восстанавливаем время
        Time.timeScale = 1f;
        // Намеренно не очищаем currentNearbyObject, чтобы можно было снова открыть диалог
    }

    public void CheckLevelCompletion()
    {
        InteractableObject[] allObjects = FindObjectsOfType<InteractableObject>(includeInactive: true);
        bool allAnomaliesCollected = true;
        bool hasNormalObjects = false;
        bool collectedNormalObject = false;

        foreach (var obj in allObjects)
        {
            if (obj.IsAnomaly && !obj.WasCollected)
            {
                allAnomaliesCollected = false;
            }

            if (!obj.IsAnomaly)
            {
                hasNormalObjects = true;
                if (obj.WasCollected)
                {
                    collectedNormalObject = true;
                }
            }
        }

        // Логика завершения уровня
        if (allAnomaliesCollected && !collectedNormalObject)
        {
            Debug.Log("Уровень пройден! Все аномалии собраны.");
            RestartLevel();
        }
        else
        {
            Debug.Log("Вы проиграли! " +
                     (collectedNormalObject ? "Был собран нормальный объект." : "Не все аномалии собраны."));
            GameOver();
        }
    }

    private void RestartLevel()
    {
        // Перезагрузка текущей сцены
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        // Здесь можно добавить логику завершения игры
        // Например, показать экран проигрыша
        Debug.Log("Game Over!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}