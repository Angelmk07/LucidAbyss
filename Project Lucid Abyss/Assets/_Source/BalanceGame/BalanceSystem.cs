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
    private bool isWin;
    private bool _isStart;
    private GameObject _ball;
    private Rigidbody2D rb;
    private float _stayTimer;
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        _isStart = true;

        if (_ball == null)
        {
            _ball = Instantiate(prefab, startPos.transform.position, Quaternion.identity);
            _ball.transform.parent = gameObject.transform;
        }
        if (rb == null)
        {
            rb = _ball.GetComponent<Rigidbody2D>();
        }
        _ball.transform.position = startPos.transform.position;
        _stayTimer = 0f;
    }

    private void Update()
    {
        if (!_isStart || _ball == null)
            return;
        if (Input.GetKeyDown(key))
        {
            rb.AddForce(Vector2.up* force, ForceMode2D.Impulse);
        }
        float distance = Vector2.Distance(startPos.transform.position, _ball.transform.position);


       if( distance < maxDistance)
        {
            _stayTimer += Time.deltaTime;
                text.text = $"{(int)_stayTimer}";
            if (_stayTimer >= needTostayTime)
            {
                isWin = true;
                GameEnd();

            }
        }
        else
        {
            isWin = false;
            GameEnd();
        }

    }
    public bool GameEnd()
    {
        _isStart = false;
        _stayTimer = 0f;
        return isWin;
        }
    private void OnDrawGizmos()
    {
        if (startPos != null)
            Gizmos.DrawWireSphere(startPos.transform.position, maxDistance);
    }
}
