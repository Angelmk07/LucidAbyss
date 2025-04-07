using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject corner1;
    [SerializeField] private GameObject corner2;
    [SerializeField] private int count;
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<Ball> container = null;
    [SerializeField] private float minDistance = 1.2f; 

    private int _nextExpectedNumber = 0;
    public event System.Action<bool> OnMinigameEnded;
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        ClearBalls();
        CreateMinigame();
        Debug.Log("create");
    }


    public void CreateMinigame()
    {
        List<Vector3> usedPositions = new();
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos;
            int attempts = 0;
            Debug.Log("create ex");
            do
            {
                spawnPos = new Vector3(
                    Random.Range(corner1.transform.position.x, corner2.transform.position.x),
                    Random.Range(corner1.transform.position.y, corner2.transform.position.y),
                    0
                );
                attempts++;
            } while (!IsFarEnough(spawnPos, usedPositions) && attempts < 100);

            if (attempts >= 100)
            {
                Debug.LogWarning("Ќе удалось найти свободное место дл€ шара!");
                continue;
            }

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            Ball ball = obj.GetComponent<Ball>();

            ball.Setup(i, this);
            container.Add(ball);
            usedPositions.Add(spawnPos);
        }
    }
    private bool IsFarEnough(Vector3 newPos, List<Vector3> existingPositions)
    {
        foreach (Vector3 pos in existingPositions)
        {
            if (Vector3.Distance(newPos, pos) < minDistance)
                return false;
        }
        return true;
    }
    public void NotifyBallClicked(Ball ball, int number)
    {
        if (number == _nextExpectedNumber)
        {
            _nextExpectedNumber++;
            ball.Hide();

            if (_nextExpectedNumber >= count)
            {
                EndMinigame(true);
            }
        }
        else
        {
            EndMinigame(false);
            ClearBalls();
        }
    }
    public void EndMinigame(bool success)
    {
        //foreach (Ball ball in container)
        //{
        //    ball.Off();
        //}
        ClearBalls();
        OnMinigameEnded?.Invoke(success);
    }



    private void ClearBalls()
    {
        foreach (Ball ball in container)
        {
            Destroy(ball.gameObject);
        }
        container.Clear();
        _nextExpectedNumber = 0;
    }
    void Restart()
    {
        _nextExpectedNumber = 0;
        foreach (Ball ball in container)
        {
            ball.Reset();
            ball.On();
        }
    }


 

    

}
