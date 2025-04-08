using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector3 poaitionBall;
    [SerializeField] private bool gravityEnabled = false;
    [SerializeField] private List<OpenDoorMiniGame> miniGameController;
    private bool gameWon;
    private Camera mainCamera;

    void Start()
    {
        poaitionBall = transform.position;
        mainCamera = Camera.main;
    }

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

        if (!IsVisibleOnScreen() && !gameWon)
        {
            if (miniGameController.Count != 0)
            {
                for (int i = 0; i < miniGameController.Count; i++)
                {
                    if (miniGameController[i] == null)
                    {
                        miniGameController[i - 1].DeactivateMiniGame();
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("You Died!");
            transform.position = poaitionBall;
            if (miniGameController.Count != 0)
            {
                for (int i = 0; i < miniGameController.Count; i++)
                {
                    if (miniGameController[i] == null)
                    {
                        miniGameController[i - 1].DeactivateMiniGame();
                    }
                }
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            transform.position = poaitionBall;
            WinGame();
        }
    }

    private void WinGame()
    {
        gameWon = true;
        transform.parent.gameObject.SetActive(false);
        Debug.Log("Игрок провел шарик к цели!");
        if (miniGameController.Count != 0)
        {
            for (int i = 0; i < miniGameController.Count; i++)
            {
                if (miniGameController[i] == null)
                {
                    miniGameController[i - 1].DeactivateMiniGame();
                }
            }
        }
    }

    private bool IsVisibleOnScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}