using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameOverUI gameOverUI = FindObjectOfType<GameOverUI>();

            if (gameOverUI != null)
            {
                CheckLevelCompletion(gameOverUI);
            }
        }
    }

    private void CheckLevelCompletion(GameOverUI gameOverUI)
    {
        InteractableObject[] allObjects = FindObjectsOfType<InteractableObject>(includeInactive: true);
        bool allAnomaliesCollected = true;
        bool collectedNormalObject = false;
        string missedAnomalies = "";
        string collectedNormalObjects = "";

        foreach (var obj in allObjects)
        {
            if (obj.IsAnomaly && !obj.WasCollected)
            {
                allAnomaliesCollected = false;
                missedAnomalies += obj.gameObject.name + ", ";
            }

            if (!obj.IsAnomaly && obj.WasCollected)
            {
                collectedNormalObject = true;
                collectedNormalObjects += obj.gameObject.name + ", ";
            }
        }

        if (missedAnomalies.Length > 0)
            missedAnomalies = missedAnomalies.Remove(missedAnomalies.Length - 2);
        if (collectedNormalObjects.Length > 0)
            collectedNormalObjects = collectedNormalObjects.Remove(collectedNormalObjects.Length - 2);

        if (allAnomaliesCollected && !collectedNormalObject)
        {
            Debug.Log("Уровень пройден! Все аномалии собраны.");
            gameOverUI.ShowGameOver("Уровень пройден! Все аномалии собраны.");
        }
        else
        {
            string errorMessage = collectedNormalObject
                ? $"Вы ошиблись: собрали нормальные объекты ({collectedNormalObjects}). В следующий раз будьте внимательней."
                : $"Вы ошиблись: не собрали аномалии ({missedAnomalies}). В следующий раз будьте внимательней.";

            Debug.Log(errorMessage);
            gameOverUI.ShowGameOver(errorMessage);
        }
    }
}