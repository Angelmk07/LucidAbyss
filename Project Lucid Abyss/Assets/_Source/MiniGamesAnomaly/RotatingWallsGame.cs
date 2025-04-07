using UnityEngine;
using TMPro;

public class RotatingWallsGame : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform wall; // Стена

    [Header("Game Settings")]
    public float rotationSpeed = 90f; // Скорость вращения стен
    public float ballCheckRadius = 0.2f; // Радиус проверки попадания шарика

    private float currentRotation = 0f;

    private void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        currentRotation += rotationInput * rotationSpeed * Time.deltaTime;

        wall.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
}