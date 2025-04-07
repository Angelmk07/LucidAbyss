using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorMiniGame : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> miniGames;
    public Transform player;
    public Transform minigame;
    public Camera camera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            camera.transform.parent = minigame;
            camera.transform.localPosition = new Vector3(0, 0, -10);
            SpawnRandomMiniGame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            camera.transform.parent = player;
            camera.transform.localPosition = new Vector3(0, 3.5f, -10);
            for (int i = 0; i < miniGames.Count; i++)
            {
                miniGames[i].SetActive(false);
            }
            Destroy(gameObject);
        }
    }

    void SpawnRandomMiniGame()
    {
        if (miniGames == null || miniGames.Count == 0)
        {
            Debug.LogWarning("Список мини-игры пуст!");
            return;
        }

        int randomIndex = Random.Range(0, miniGames.Count);
        GameObject itemToSpawn = miniGames[randomIndex];
        itemToSpawn.SetActive(true);
        Debug.Log($"Сейчас мини-игра запущена: {itemToSpawn.name}");
    }
}