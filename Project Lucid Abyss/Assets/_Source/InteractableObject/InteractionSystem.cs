using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionSystem : MonoBehaviour
{
    [Header("MiniGames")]
    [SerializeField] private GameObject tugPanel;
    [SerializeField] private GameObject ballSubsequence;
    [SerializeField] private GameObject skillCheck;
    [SerializeField] private GameObject MiniGamesBackGround;
    [Header("UI References")]

    //[SerializeField] private TMP_Text questionText;
    //[SerializeField] private Button yesButton;
    //[SerializeField] private Button noButton;
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
    private ScretchController _scretchController;
    private BallSpawner _ballSpawner;
    private LockPickingGame _lockPicking;
    private GameTimer _gameTimer;

    private void Awake()
    {
        tugPanel.SetActive(false);

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

        //yesButton.onClick.AddListener(OnYesClicked);
        //noButton.onClick.AddListener(OnNoClicked);
    }

    private void Update()
    {
        CheckNearbyObjects();
        UpdateInteractionPrompt();
    }

    public void Constructor(ScretchController scretchController, GameTimer gameTimer,BallSpawner ballSpawner, LockPickingGame lockPicking)
    {
        _scretchController = scretchController;
        _gameTimer = gameTimer;
        _ballSpawner = ballSpawner;
        _lockPicking = lockPicking;
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

        float targetAlpha = (hasNearbyObject && !tugPanel.activeSelf) ? 1f : 0f;
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
            StartMiniGame(currentNearbyObject);
        }
    }
    private void StartMiniGame(InteractableObject obj)
    {
        MiniGameType gameType = (MiniGameType)Random.Range(0, 3);
        switch (gameType)
        {
            case MiniGameType.Tug:
                StartUIMiniGame(obj);
                break;
            case MiniGameType.Skillcheck:
                StartWorldMiniGame1(obj);
                break;
            case MiniGameType.Subsequence:
                StartWorldMiniGame2(obj);
                break;
        }
    }

    private void StartWorldMiniGame2(InteractableObject obj)
    {
        ballSubsequence.SetActive(true);
        MiniGamesBackGround.SetActive(true);
        _ballSpawner.StartGame();
        _ballSpawner.OnMinigameEnded += (bool result) =>
        {
            if (result)
            {
                MiniGamesBackGround.SetActive(false);
                currentNearbyObject?.Collect();
                if (!obj.IsAnomaly)
                {
                    _gameTimer.AddFouldCost();
                }
            }
            MiniGamesBackGround.SetActive(false);
        };
   
    }

    private void StartWorldMiniGame1(InteractableObject obj)
    {
        skillCheck.SetActive(true);
        MiniGamesBackGround.SetActive(true);
        _lockPicking.StartMiniGame();
        _lockPicking.OnGameEnd += (bool result) =>
        {
            if (result)
            {

                currentNearbyObject?.Collect();
                if (!obj.IsAnomaly)
                {
                    _gameTimer.AddFouldCost();
                }
            }
            MiniGamesBackGround.SetActive(false);
        };

    }

    private void StartUIMiniGame(InteractableObject obj)
    {

        tugPanel.SetActive(true);
        currentNearbyObject = obj;
        _scretchController.GetAnomalyInfo(obj.AnomalyInfo);
        _scretchController.onGameEnd += (bool result) =>
        {
            if (result)
            {
               
                currentNearbyObject?.Collect();
                if (!obj.IsAnomaly)
                {
                    _gameTimer.AddFouldCost();
                }
            }
            tugPanel.SetActive(false);
        };


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
        tugPanel .SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
public enum MiniGameType
{
    Tug,
    Skillcheck,
    Subsequence
}