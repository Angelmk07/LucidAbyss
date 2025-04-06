using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System;

public class LocaleDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.name));
        }
        dropdown.options = options;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        dropdown.value = 0;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}