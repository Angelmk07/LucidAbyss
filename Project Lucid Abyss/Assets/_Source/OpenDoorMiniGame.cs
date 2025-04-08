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
    public float activationDistance = 2f;

    private bool isMiniGameActive = false;

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= activationDistance && !isMiniGameActive && Input.GetKeyDown(KeyCode.E))
        {
            ActivateMiniGame();
        }
    }

    void ActivateMiniGame()
    {
        InputListener inputListener = FindAnyObjectByType<InputListener>();
        inputListener.enabled = false;
        isMiniGameActive = true;
        camera.transform.parent = minigame;
        camera.transform.localPosition = new Vector3(0, 0, -10);
        SpawnRandomMiniGame();
    }

    public void DeactivateMiniGame()
    {
        InputListener inputListener = FindAnyObjectByType<InputListener>();
        inputListener.enabled = true;
        isMiniGameActive = false;
        camera.transform.parent = player;
        camera.transform.localPosition = new Vector3(0, 3.5f, -10);

        for (int i = 0; i < miniGames.Count; i++)
        {
            miniGames[i].SetActive(false);
        }
        Destroy(this.gameObject);
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