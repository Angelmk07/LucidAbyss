using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool isAnomaly = false;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color anomalyColor = Color.red;

    private SpriteRenderer spriteRenderer;

    public bool IsAnomaly => isAnomaly;
    public bool WasCollected { get; private set; } = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    private void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            UpdateColor();
    }

    //public void GetAnomalyInfo(AnomalySO anomalySO)
    //{
    //    anomalyData = anomalySO;
    //    if (anomalyData != null)
    //    {
    //        _model.Initialize(
    //             anomalySO.CounterForce,
    //             anomalyData.NeedPowerMoreThan,
    //             anomalyData.TimeToScretch
    //        );
    //        isGameActive = true;
    //    }
    //}

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isAnomaly ? anomalyColor : normalColor;
        }
    }

    public void Collect()
    {
        WasCollected = true;
        gameObject.SetActive(false);
    }
}