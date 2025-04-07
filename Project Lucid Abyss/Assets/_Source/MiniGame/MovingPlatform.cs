using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 5f;
    private float startPositionX;
    public int direction = 1;

    void Start()
    {
        startPositionX = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x > startPositionX + distance)
        {
            direction = -1;
        }
        else if (transform.position.x < startPositionX - distance)
        {
            direction = 1;
        }

        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }
}