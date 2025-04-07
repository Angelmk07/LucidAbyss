using DG.Tweening;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float duration = 1.5f;

    private SpriteRenderer renderer;
    private int number;
    private BallSpawner spawner;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(int number, BallSpawner spawner)
    {
        this.number = number;
        this.spawner = spawner;
        text.text = $"{number}";
    }

    private void OnMouseDown()
    {
        spawner.NotifyBallClicked(this, number);
    }

   

    public void Hide()
    {
        renderer.DOFade(0, duration);
        text.DOFade(0, duration);
    }

    public void Reset()
    {
        renderer.DOFade(1, 0);
        text.DOFade(1, 0);
    }
    public void On()
    {
        gameObject.SetActive(true);
    }
    public void Off()
    {
        gameObject.SetActive(false);
    }
}
