using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _translationKey;

    private TMP_FontAsset _latinFontAsset;
    private float _latinFontSize;

    [Header("Cyrillic Font")]
    [SerializeField] private TMP_FontAsset _cyrillicFontAsset;
    [SerializeField][Min(0f)] private float _cyrillicFontSize;

    private TMP_Text _tmpText;

    private void Start()
    {
        _tmpText = GetComponent<TMP_Text>();

        _latinFontAsset = _tmpText.font;
        _latinFontSize = _tmpText.fontSize;

        UpdateText();
    }

    public void UpdateText()
    {
        Language currentLanguage = TranslationManager.Instance.CurrentLanguage;

        if (currentLanguage is Language.Русский && _cyrillicFontAsset != null)
        {
            _tmpText.font = _cyrillicFontAsset;
            _tmpText.fontSize = _cyrillicFontSize;
        }
        else
        {
            _tmpText.font = _latinFontAsset;
            _tmpText.fontSize = _latinFontSize;
        }

        _tmpText.text = TranslationManager.Translations[_translationKey][currentLanguage];
    }
}
