using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockPickingGame : MonoBehaviour
{
    [Header("Game Settings")]
    public float rotationSpeed = 180f; // Скорость вращения стрелки
    public float successZoneAngle = 45f; // Угол успешной зоны

    [Header("References")]
    public RectTransform needle; // Стрелка
    public Image successZone; // Красная зона успеха
    public TMP_Text resultText; // Текст результата
    public GameObject gamePanel; // Панель игры

    private bool isRotating = true;
    private bool gameWon = false;

    private void Start()
    {
        successZone.fillAmount = successZoneAngle / 360f;
        successZone.transform.rotation = Quaternion.Euler(0, 0, -successZoneAngle / 2);

        resultText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isRotating)
        {
            needle.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                CheckForSuccess();
            }
        }
    }

    private void CheckForSuccess()
    {
        float needleAngle = needle.eulerAngles.z % 360;
        if (needleAngle < 0) needleAngle += 360;

        float zoneStart = (360 - successZoneAngle / 2) % 360;
        float zoneEnd = (successZoneAngle / 2) % 360;

        bool inSuccessZone;

        if (zoneStart < zoneEnd)
        {
            inSuccessZone = (needleAngle >= zoneStart && needleAngle <= zoneEnd);
        }
        else
        {
            inSuccessZone = (needleAngle >= zoneStart || needleAngle <= zoneEnd);
        }

        if (inSuccessZone)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        isRotating = false;
        gameWon = true;
        resultText.text = "УСПЕХ!";
        resultText.color = Color.green;
        resultText.gameObject.SetActive(true);

        Debug.Log("Замок взломан!");
    }

    private void LoseGame()
    {
        isRotating = false;
        gameWon = false;
        resultText.text = "ПРОВАЛ";
        resultText.color = Color.red;
        resultText.gameObject.SetActive(true);

        Invoke("RestartGame", 2f);
    }

    private void RestartGame()
    {
        isRotating = true;
        resultText.gameObject.SetActive(false);
        needle.rotation = Quaternion.identity;
    }

    public bool IsGameWon()
    {
        return gameWon;
    }
}