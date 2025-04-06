using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class PhraseView : MonoBehaviour
{
    [SerializeField] private int timeToHide = 7;
    [SerializeField] private LocalizedString[] localizedStrings;

    private LocalizeStringEvent _phraseText;
    private int _currentLevel;

    void Start()
    {
        _phraseText = GetComponent<LocalizeStringEvent>();
        _currentLevel = PlayerPrefs.GetInt("Level", 0);
        if (_currentLevel >= localizedStrings.Length)
            _currentLevel = 0;
        ShowPhrase();
    }

    private void ShowPhrase()
    {
        _phraseText.gameObject.SetActive(true);
        _phraseText.StringReference = localizedStrings[_currentLevel];
        PlayerPrefs.SetInt("Level", _currentLevel + 1);
        StartCoroutine(HidePhrase());
    }

    private IEnumerator HidePhrase()
    {
        yield return new WaitForSeconds(timeToHide);
        _phraseText.gameObject.SetActive(false);
    }
}
