using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceSystem : MonoBehaviour
{
    [SerializeField] private GameObject startPos;
    [SerializeField] private GameObject prefab;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float force = 5f;
    [SerializeField] private float needTostayTime = 2f;
    [SerializeField] private KeyCode key = KeyCode.Space;
    [SerializeField] private List<OpenDoorMiniGame> miniGameController;

    private bool isWin;
    private bool _isStart;
    private GameObject _ball;
    private Rigidbody2D rb;
    private float _stayTimer;

    private void OnEnable()
    {
        StartGame();
    }

    public void StartGame()
    {
        _isStart = true;
        text.text = "0";
        isWin = false;

        if (_ball == null)
        {
            _ball = Instantiate(prefab, startPos.transform.position, Quaternion.identity);
        }

        if (rb == null)
        {
            rb = _ball.GetComponent<Rigidbody2D>();
        }

        _ball.transform.position = startPos.transform.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        _stayTimer = 0f;
    }

    private void Update()
    {
        if (!_isStart || _ball == null)
            return;

        if (Input.GetKeyDown(key))
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        float distance = Vector2.Distance(startPos.transform.position, _ball.transform.position);

        if (distance < maxDistance)
        {
            _stayTimer += Time.deltaTime;
            text.text = $"{(int)_stayTimer}";

            if (_stayTimer >= needTostayTime)
            {
                EndGame(true);
            }
        }
        else
        {
            EndGame(false);
        }
    }

    private void EndGame(bool win)
    {
        isWin = win;
        _isStart = false;
        _stayTimer = 0f;

        if (_ball != null)
        {
            Destroy(_ball);
            _ball = null;
            rb = null;
        }
        if (miniGameController.Count != 0)
        {
            for (int i = 1; i < miniGameController.Count; i++)
            {
                if (miniGameController[i] == null)
                {
                    miniGameController[i - 1].DeactivateMiniGame();
                }
            }
        }
        else
        {
            miniGameController[0].DeactivateMiniGame();
        }

        text.text = isWin ? "Win" : "Fail";
    }

    public bool GameEnd()
    {
        return isWin;
    }

    private void OnDrawGizmos()
    {
        if (startPos != null)
            Gizmos.DrawWireSphere(startPos.transform.position, maxDistance);
    }
}