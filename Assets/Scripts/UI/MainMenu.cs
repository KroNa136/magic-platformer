using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private const string CurrentLevelKey = "current_level";
    private const string HealthKey = "player_health";
    private const string ManaKey = "player_mana";
    private const string IntellectKey = "player_intellect";
    private const string CoinsKey = "player_coins";

    private readonly Color EnabledButtonTextColor = new Color32(0xD3, 0xAD, 0xA7, 0xFF);
    private readonly Color DisabledButtonTextColor = new Color32(0xC4, 0xC4, 0xC4, 0xFF);

    [SerializeField] private Button _continueButton;
    [SerializeField] private TMP_Text _continueButtonText;
    [SerializeField] private SettingsModal _settingsModal;

    private int _currentLevel;

    private void Start()
    {
        bool firstLaunch = !PlayerPrefs.HasKey(CurrentLevelKey);

        if (firstLaunch)
        {
            _currentLevel = 1;

            _continueButton.interactable = false;
            _continueButtonText.color = DisabledButtonTextColor;
        }
        else
        {
            _currentLevel = PlayerPrefs.GetInt(CurrentLevelKey);

            _continueButton.interactable = true;
            _continueButtonText.color = EnabledButtonTextColor;
        }
    }

    public void Continue()
        => LoadLevel(_currentLevel);

    public void NewGame()
    {
        _currentLevel = 1;

        PlayerPrefs.SetInt(CurrentLevelKey, _currentLevel);
        PlayerPrefs.DeleteKey(HealthKey);
        PlayerPrefs.DeleteKey(ManaKey);
        PlayerPrefs.DeleteKey(IntellectKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.Save();

        LoadLevel(_currentLevel);
    }

    public void Settings()
        => _settingsModal.Activate();

    public void Quit()
        => Application.Quit();

    private void LoadLevel(int level)
        => SceneManager.LoadSceneAsync($"Level {level}", LoadSceneMode.Single);
}
