using System;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool gravityEnabled = false;

    [Header("UI")]
    public TMP_Text resultText; // Текст результата
    //public GameObject gamePanel; // Панель игры

    private bool gameWon;

    //private void Awake()
    //{
    //    gamePanel.SetActive(false);
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gravityEnabled = !gravityEnabled;
            rb.gravityScale = gravityEnabled ? 1 : 0;

            if (!gravityEnabled)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            LostGame();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            WinGame();
        }
    }
    private void OnBecameInvisible()
    {
        LostGame();
    }

    private void LostGame()
    {
        gameWon = false;
        transform.parent.gameObject.SetActive(false);
        InputListener.CanDo.Invoke(true);
    }

    private void WinGame()
    {
        gameWon = true;
        transform.parent.gameObject.SetActive(false);

        Debug.Log("Игрок провел шарик к цели!");
        InputListener.CanDo.Invoke(true);
    }
}