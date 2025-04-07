using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool gravityEnabled = false;
    private OpenDoorMiniGame miniGameController;
    private bool gameWon;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        miniGameController = FindObjectOfType<OpenDoorMiniGame>();
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
            miniGameController?.DeactivateMiniGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("You Died!");
            miniGameController?.DeactivateMiniGame();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameWon = true;
        transform.parent.gameObject.SetActive(false);
        Debug.Log("Игрок провел шарик к цели!");
        miniGameController?.DeactivateMiniGame();
    }

    private bool IsVisibleOnScreen()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}