using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [field:SerializeField] public AnomalySO AnomalyInfo { get; private set; }
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