using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    public void ChangeLanguage(int localeID)
    {
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;
        if (_localeID >= 0 && _localeID < locales.Count)
        {
            LocalizationSettings.SelectedLocale = locales[_localeID];
            // Debug.Log("Language changed to: " + locales[_localeID].LocaleName);
        }
        else
        {
            // Debug.LogError("Invalid language ID!");
        }
    }
}