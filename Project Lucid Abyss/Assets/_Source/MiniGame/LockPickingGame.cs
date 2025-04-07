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

    private void Start()
    {
        if (correctRotation == 0f)
        {
            correctRotation = Random.Range(30f, 330f);
        }
    }

    private void Update()
    {
        if (isSolved) return;

        currentRotation += rotationSpeed * rotationDirection * Time.deltaTime;
        currentRotation = Mathf.Repeat(currentRotation, 360f);

        if (lockTransform != null)
        {
            lockTransform.localEulerAngles = new Vector3(0, 0, currentRotation);
        }

        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentRotation, correctRotation));
        isInCorrectZone = (angleDifference <= snapThreshold);

        if (isInCorrectZone && Input.GetKeyDown(KeyCode.Space))
        {
            isSolved = true;
            gameObject.SetActive(false);
            Debug.Log("Замок взломан! Угол: " + currentRotation.ToString("0.0") + "°");
        }

        if (!isInCorrectZone && Input.GetKeyDown(KeyCode.Space))
        {
            rotationDirection *= -1;
        }
    }
}