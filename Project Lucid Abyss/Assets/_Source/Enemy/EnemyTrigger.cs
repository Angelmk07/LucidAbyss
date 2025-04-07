using System.Collections;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private int enemyLifeTime;
    [SerializeField] private LayerMask playerLayrMask;
    [SerializeField] private GameObject tiger;
    [SerializeField] private GameTimer timer;

    private int _playerLayer;
    private bool isSpawned;

    void Start()
    {
        _playerLayer = (int)Mathf.Log(playerLayrMask.value, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _playerLayer && !tiger.activeInHierarchy && !isSpawned)
            StartCoroutine(EnemyLifeTime());
    }

    private IEnumerator EnemyLifeTime()
    {
        tiger.SetActive(true);
        timer.Stop();
        isSpawned = true;
        yield return new WaitForSeconds(enemyLifeTime);
        tiger.SetActive(false);
        timer.Continue();
    }
}
