using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (tutorialPanel != null)
            {
                Time.timeScale = 0;
                tutorialPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tutorialPanel != null)
        {
            Time.timeScale = 1;
            Destroy(tutorialPanel);
        }
        door.SetActive(false);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            Time.timeScale = 1;
            Destroy(tutorialPanel);
        }
        door.SetActive(false);
    }
}