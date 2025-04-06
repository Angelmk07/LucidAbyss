using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScretchView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color unSuccessColor;

    public float SliderValue
    {
        get => slider.value;
        set => slider.value = value;
    }

    public float MaxValue => slider.maxValue;

    public void SetSuccessVisual()
    {
        image.color = successColor;
    }

    public void SetUnSuccessVisual()
    {
        image.color = unSuccessColor;
    }

    public void SetTimerText(float timeLeft)
    {
        timerText.text = timeLeft.ToString("F1");
    }
}
