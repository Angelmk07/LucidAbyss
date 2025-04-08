using UnityEngine;
using UnityEngine.UI;

public class TutorialByLayerSystem : MonoBehaviour
{
    [Header("Tutorial Panels")]
    [SerializeField] private GameObject DoorTutorialPanel;
    [SerializeField] private GameObject AnomalyTutorialPanel;
    [SerializeField] private Button Close;
    [SerializeField] private Button Close_2;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask DoorLayer;
    [SerializeField] private LayerMask AnomalyLayer;

    private bool DoorShown = false;
    private bool AnomalyShown = false;
    private void Awake()
    {
        Close.onClick.AddListener(CloseAllTutorials);
        Close_2.onClick.AddListener(CloseAllTutorials);
    }
    public void TryShowTutorial(GameObject obj)
    {
        int objLayer = obj.layer;

        if (IsInLayerMask(objLayer, DoorLayer) && !DoorShown)
        {
            DoorShown = true;
            ShowPanel(DoorTutorialPanel);
        }
        else if (IsInLayerMask(objLayer, AnomalyLayer) && !AnomalyShown)
        {
            AnomalyShown = true;
            ShowPanel(AnomalyTutorialPanel);
        }

    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseAllTutorials()
    {
        DoorTutorialPanel.SetActive(false);
        AnomalyTutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return ((mask.value & (1 << layer)) != 0);
    }
}
