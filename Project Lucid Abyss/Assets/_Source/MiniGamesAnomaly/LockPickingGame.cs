using UnityEngine;

public class LockPickingGame : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 30f;
    public float correctRotation = 90f;
    public float snapThreshold = 15f;
    public float rotationDirection = 1;

    [Header("References")]
    public Transform lockTransform;

    private float currentRotation = 0f;
    private bool isInCorrectZone = false;
    private bool isSolved = false;
    private bool isActive = false;

    public System.Action<bool> OnGameEnd;

    private void Start()
    {
        StartMiniGame();
    }

    private void Update()
    {
        if (!isActive || isSolved) return;

        currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;
        currentRotation = Mathf.Repeat(currentRotation, 360f);

        if (lockTransform != null)
        {
            lockTransform.localEulerAngles = new Vector3(0, 0, currentRotation);
        }

        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentRotation, correctRotation));
        isInCorrectZone = (angleDifference <= snapThreshold);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInCorrectZone)
            {
                Debug.Log("Замок взломан! Угол: " + currentRotation.ToString("0.0") + "°");
                EndGame(true);
            }
            else
            {
                Debug.Log("Ошибка! Угол: " + currentRotation.ToString("0.0") + "°");
                EndGame(false);
            }
        }
    }

    public void StartMiniGame()
    {
        isSolved = false;
        isActive = true;
        gameObject.SetActive(true);
        currentRotation = 0f;

        if (lockTransform != null)
        {
            lockTransform.localEulerAngles = Vector3.zero;
        }
    }

    private void EndGame(bool success)
    {
        isSolved = true;
        isActive = false;
        gameObject.SetActive(false);
        OnGameEnd?.Invoke(success);
    }

    public void ResetGame()
    {
        isSolved = false;
        isActive = false;
        currentRotation = 0f;
        lockTransform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
    }
}
