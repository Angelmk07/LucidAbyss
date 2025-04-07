using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool gravityEnabled = false;

    private bool gameWon;

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
            Debug.Log("You Died!");
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
    }
}