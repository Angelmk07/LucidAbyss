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

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void Update()
    {
        CheckNearbyObjects();
        UpdateInteractionPrompt();
    }

    //Метод для создание области проверки на объект
    private void CheckNearbyObjects()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange, interactableLayer);
        hasNearbyObject = false;
        currentNearbyObject = null;

        foreach (var collider in hitColliders)
        {
            var obj = collider.GetComponent<InteractableObject>();
            if (obj != null && !obj.WasCollected)
            {
                hasNearbyObject = true;
                currentNearbyObject = obj;
                break;
            }
        }
    }

    //Метод для создание анимации появления/исчезновения текста
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

            interactionPromptText.text = $"Нажмите {KeyCode.E} чтобы взаимодействовать";
            UpdatePromptPosition();
        }
        else if (interactionPrompt.activeSelf)
        {
            interactionPrompt.SetActive(false);
        }
    }

    //Метод для установления позии текста
    private void UpdatePromptPosition()
    {
        if (Camera.main != null)
        {
            Vector3 worldPosition = transform.position + Vector3.up * promptHeight;
            interactionPrompt.transform.position = worldPosition;
        }
    }

    //Метод для вызова экрана пройгрыша
    public void TryInteract()
    {
        if (hasNearbyObject && currentNearbyObject != null && !currentNearbyObject.WasCollected)
        {
            ShowInteractionDialog(currentNearbyObject);
        }
    }

    //Метод для показа текста с названием аномалии
    private void ShowInteractionDialog(InteractableObject obj)
    {
        currentNearbyObject = obj;
        questionText.text = $"Вы хотите взять этот предмет? (Аномалия: {(obj.IsAnomaly ? "Да" : "Нет")})";
        interactionPanel.SetActive(true);

        Time.timeScale = 0f;

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
        Time.timeScale = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}