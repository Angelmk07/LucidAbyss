using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprint;
    [SerializeField] private float hearingRadius;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask playerLayrMask;

    private bool isSeePlayer;
    Collider2D _player;

    private int _playerLayer;
    void Start()
    {
        _playerLayer = (int)Mathf.Log(playerLayrMask.value, 2);
    }

    void Update()
    {
        _player = Physics2D.OverlapCircle(transform.position, hearingRadius, playerLayrMask);
        if(_player?.GetComponent<Rigidbody2D>().velocity.x != 0)
            isSeePlayer = true;
        else
            isSeePlayer = false;
    }

    private void FixedUpdate()
    {
        if (!isSeePlayer)
            rb.velocity = new Vector2(speed, 0);
        else
        {
            if(transform.position.x < _player?.transform.position.x)
                rb.velocity = new Vector2(sprint * -1, 0);
            else
                rb.velocity = new Vector2(speed, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer && isSeePlayer)
            Debug.Log("GameOver");
    }
}
