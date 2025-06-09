using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinishedModal : Modal
{
    private const string CurrentLevelKey = "current_level";
    private const string HealthKey = "player_health";
    private const string ManaKey = "player_mana";
    private const string IntellectKey = "player_intellect";
    private const string CoinsKey = "player_coins";

    public void ToMenu()
    {
        PlayerPrefs.DeleteKey(CurrentLevelKey);
        PlayerPrefs.DeleteKey(HealthKey);
        PlayerPrefs.DeleteKey(ManaKey);
        PlayerPrefs.DeleteKey(IntellectKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.Save();

        Deactivate();
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }

    protected override void OnActivate() { }
    protected override void OnDeactivate() { }
}
