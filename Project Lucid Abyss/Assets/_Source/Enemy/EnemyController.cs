using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprint;
    [SerializeField] private float hearingRadius;
    [SerializeField] private LayerMask playerLayrMask;
    [SerializeField] private GameOverUI gameOverUI;

    private bool isSeePlayer;
    private Collider2D _player;
    private int _playerLayer;

    void Start()
    {
        _playerLayer = (int)Mathf.Log(playerLayrMask.value, 2);
    }

    void Update()
    {
        _player = Physics2D.OverlapCircle(transform.position, hearingRadius, playerLayrMask);
        if (_player?.GetComponent<Rigidbody2D>().velocity.x != 0)
            isSeePlayer = true;
        else
            isSeePlayer = false;

        MoveEnemy();
    }

    private void MoveEnemy()
    {
        float moveSpeed = speed;
        Vector3 direction = Vector3.right;

        if (!isSeePlayer)
        {
            direction = Vector3.right * Mathf.Sign(-speed);
        }
        else if (_player != null)
        {
            if (transform.position.x < _player.transform.position.x)
            {
                moveSpeed = sprint;
                direction = Vector3.right * -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                moveSpeed = speed;
                direction = Vector3.right;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer && isSeePlayer)
            gameOverUI.ShowGameOver("Был растерзан тигром");
    }
}
