using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : HealthManager, IHealable
{
    public static PlayerHealthManager Instance;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private GameOverModal _gameOverModal;

    protected override void OnAwake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    protected override void OnStart()
    {
        _maxAmount = PlayerStats.Instance.Health;

        _healthSlider.minValue = 0;
        _healthSlider.maxValue = Mathf.Round(_maxAmount);

        OnCurrentAmountChange.AddListener(newAmount =>
        {
            _healthSlider.value = Mathf.Round(newAmount);
            _healthText.text = $"{Mathf.Round(newAmount)} / {_maxAmount}";
        });
    }

    public void Heal(float amount)
    {
        CurrentAmount = Mathf.Clamp(CurrentAmount + amount, 0f, _maxAmount);
    }

    public void UpdateMaxAmount(float newAmount)
    {
        float scaledCurrentAmount = CurrentAmount / _maxAmount * newAmount;

        _healthSlider.minValue = 0;
        _healthSlider.maxValue = Mathf.Round(newAmount);

        _maxAmount = newAmount;
        CurrentAmount = scaledCurrentAmount;
    }

    protected override void OnDeath()
    {
        PlayerMovement.Instance.DisableMovement();
        _gameOverModal.Activate();
    }
}
