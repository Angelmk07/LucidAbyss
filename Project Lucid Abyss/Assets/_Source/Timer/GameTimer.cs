using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Slider))]
public class GameTimer : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float fouldCost = 5;
    [SerializeField] private Image image;
    [Header("Shake Settings")]
    [SerializeField] private ShakeData[] shakeStages;
    private Vector3 _initialPosition;
    private Slider _slider;
    private float _lastShakeTime;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _initialPosition = _slider.transform.localPosition;
        System.Array.Sort(shakeStages, (a, b) => a.percentThreshold.CompareTo(b.percentThreshold));
    }

    private void Update()
    {
        _slider.value += Time.deltaTime;
        float currentPercent = (_slider.value / _slider.maxValue) * 100f;

        for (int i = shakeStages.Length - 1; i >= 0; i--)
        {
            if (currentPercent > shakeStages[i].percentThreshold)
            {
                TryShake(shakeStages[i]);
                return;
            }
            else
            {
                image.color = shakeStages[i].color;
            }
        }
    }
    public void AddFouldCost()
    {
        _slider.value += fouldCost;
    }

    private void TryShake(ShakeData shakeData)
    {
        if (Time.time - _lastShakeTime > shakeData.time)
        {
            _slider.transform
                .DOShakePosition(duration, shakeData.power)
                .OnComplete(() => _slider.transform.localPosition = _initialPosition);

            _lastShakeTime = Time.time;
        }
    }
}

[System.Serializable]
public class ShakeData
{
    public float power;
    public float time;
    public float percentThreshold;
    public Color color;
}