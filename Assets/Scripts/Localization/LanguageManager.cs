using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _selectLanguageDropdown;

    private LocalizedText[] _localizedTexts;

    private void Start()
    {
        _localizedTexts = FindObjectsByType<LocalizedText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (_selectLanguageDropdown != null)
        {
            var languages = new List<string>(Enum.GetNames(typeof(Language)));

            _selectLanguageDropdown.ClearOptions();
            _selectLanguageDropdown.AddOptions(languages);

            StartCoroutine(SetDropdownValueAfterDelay());
        }
    }

    public void SetLanguage()
    {
        PlayerPrefs.SetInt("language", _selectLanguageDropdown.value);

        foreach (var localizedText in _localizedTexts)
            localizedText.UpdateText();
    }

    private IEnumerator SetDropdownValueAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _selectLanguageDropdown.value = (int) TranslationManager.Instance.CurrentLanguage;
    }
}
