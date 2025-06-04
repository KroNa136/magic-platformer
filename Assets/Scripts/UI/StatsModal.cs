using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsModal : Modal
{
    private readonly Color EnabledButtonTextColor = new Color32(0xD3, 0xAD, 0xA7, 0xFF);
    private readonly Color DisabledButtonTextColor = new Color32(0xC4, 0xC4, 0xC4, 0xFF);

    [SerializeField] private TMP_Text _levelText;

    [Space]

    [SerializeField] private Slider _healthProgressBar;
    [SerializeField] private TMP_Text _healthProgressText;
    [SerializeField] private Button _healthUpgradeButton;
    [SerializeField] private TMP_Text _healthUpgradeButtonText;

    [Space]

    [SerializeField] private Slider _manaProgressBar;
    [SerializeField] private TMP_Text _manaProgressText;
    [SerializeField] private Button _manaUpgradeButton;
    [SerializeField] private TMP_Text _manaUpgradeButtonText;

    [Space]

    [SerializeField] private Slider _intellectProgressBar;
    [SerializeField] private TMP_Text _intellectProgressText;
    [SerializeField] private Button _intellectUpgradeButton;
    [SerializeField] private TMP_Text _intellectUpgradeButtonText;

    private void Start()
    {
        _healthProgressBar.minValue = 0;
        _healthProgressBar.value = 0;
        _healthProgressBar.maxValue = PlayerStats.MaxUpgradeLevel;

        _manaProgressBar.minValue = 0;
        _manaProgressBar.value = 0;
        _manaProgressBar.maxValue = PlayerStats.MaxUpgradeLevel;

        _intellectProgressBar.minValue = 0;
        _intellectProgressBar.value = 0;
        _intellectProgressBar.maxValue = PlayerStats.MaxUpgradeLevel;
    }

    public void UpgradeHealth()
    {
        if (CoinManager.Instance.TrySpendCoins(PlayerStats.Instance.HealthUpgradePrice))
        {
            PlayerStats.Instance.UpgradeHealth();
            UpdateVisuals();
        }
    }

    public void UpgradeMana()
    {
        if (CoinManager.Instance.TrySpendCoins(PlayerStats.Instance.ManaUpgradePrice))
        {
            PlayerStats.Instance.UpgradeMana();
            UpdateVisuals();
        }
    }

    public void UpgradeIntellect()
    {
        if (CoinManager.Instance.TrySpendCoins(PlayerStats.Instance.IntellectUpgradePrice))
        {
            PlayerStats.Instance.UpgradeIntellect();
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        int currentHealthUpgradeLevel = PlayerStats.Instance.CurrentHealthValueIndex;
        int currentManaUpgradeLevel = PlayerStats.Instance.CurrentManaValueIndex;
        int currentIntellectUpgradeLevel = PlayerStats.Instance.CurrentIntellectValueIndex;

        int healthUpgradePrice = PlayerStats.Instance.HealthUpgradePrice;
        int manaUpgradePrice = PlayerStats.Instance.ManaUpgradePrice;
        int intellectUpgradePrice = PlayerStats.Instance.IntellectUpgradePrice;

        int currentUpgradeLevel = 1 + currentHealthUpgradeLevel + currentManaUpgradeLevel + currentIntellectUpgradeLevel;
        _levelText.text = $"Level {currentUpgradeLevel}";

        _healthProgressBar.value = currentHealthUpgradeLevel;
        _healthProgressText.text = $"{currentHealthUpgradeLevel} / {PlayerStats.MaxUpgradeLevel}";

        if (healthUpgradePrice == -1)
        {
            _healthUpgradeButton.gameObject.SetActive(false);
        }
        else
        {
            bool canUpgradeHealth = CoinManager.Instance.CurrentAmount >= healthUpgradePrice;

            _healthUpgradeButton.gameObject.SetActive(true);
            _healthUpgradeButton.interactable = canUpgradeHealth;

            _healthUpgradeButtonText.text = $"{healthUpgradePrice}";
            _healthUpgradeButtonText.color = canUpgradeHealth ? EnabledButtonTextColor : DisabledButtonTextColor;
        }

        _manaProgressBar.value = currentManaUpgradeLevel;
        _manaProgressText.text = $"{currentManaUpgradeLevel} / {PlayerStats.MaxUpgradeLevel}";

        if (manaUpgradePrice == -1)
        {
            _manaUpgradeButton.gameObject.SetActive(false);
        }
        else
        {
            bool canUpgradeMana = CoinManager.Instance.CurrentAmount >= manaUpgradePrice;

            _manaUpgradeButton.gameObject.SetActive(true);
            _manaUpgradeButton.interactable = canUpgradeMana;

            _manaUpgradeButtonText.text = $"{manaUpgradePrice}";
            _manaUpgradeButtonText.color = canUpgradeMana ? EnabledButtonTextColor : DisabledButtonTextColor;
        }

        _intellectProgressBar.value = currentIntellectUpgradeLevel;
        _intellectProgressText.text = $"{currentIntellectUpgradeLevel} / {PlayerStats.MaxUpgradeLevel}";

        if (intellectUpgradePrice == -1)
        {
            _intellectUpgradeButton.gameObject.SetActive(false);
        }
        else
        {
            bool canUpgradeIntellect = CoinManager.Instance.CurrentAmount >= intellectUpgradePrice;

            _intellectUpgradeButton.gameObject.SetActive(true);
            _intellectUpgradeButton.interactable = canUpgradeIntellect;

            _intellectUpgradeButtonText.text = $"{intellectUpgradePrice}";
            _intellectUpgradeButtonText.color = canUpgradeIntellect ? EnabledButtonTextColor : DisabledButtonTextColor;
        }
    }

    protected override void OnActivate()
        => UpdateVisuals();

    protected override void OnDeactivate() { }
}
