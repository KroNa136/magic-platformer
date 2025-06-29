using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private const string CoinsKey = "player_coins";

    [SerializeField] private TMP_Text _coinsText;

    private int _currentAmount;
    public int CurrentAmount
    {
        get => _currentAmount;
        private set
        {
            _coinsText.text = $"{value}";
            _currentAmount = value;
        }
    }

    private PlayerAudioController _audioController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        TryGetComponent(out _audioController);
    }

    private void Start()
    {
        bool newGame = !PlayerPrefs.HasKey(CoinsKey);

        if (newGame)
        {
            CurrentAmount = 0;
            Save();
        }
        else
        {
            CurrentAmount = PlayerPrefs.GetInt(CoinsKey);
        }
    }

    public void AddCoin()
    {
        CurrentAmount++;
        Save();

        _audioController.Bind(audioController => audioController.PickCoin());
    }

    public bool TrySpendCoins(int amount)
    {
        if (CurrentAmount < amount)
            return false;

        CurrentAmount -= amount;
        Save();

        _audioController.Bind(audioController => audioController.SpendCoins());

        return true;
    }

    private void Save()
    {
        PlayerPrefs.SetInt(CoinsKey, CurrentAmount);
        PlayerPrefs.Save();
    }
}
