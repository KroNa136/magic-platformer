using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    public static ManaManager Instance;

    [SerializeField] private Slider _manaSlider;
    [SerializeField] private TMP_Text _manaText;
    [SerializeField][Min(0f)] private float _maxAmount = 100f;
    [SerializeField][Min(0f)] private float _regenerationSpeed = 10f;

    private float _currentAmount;
    public float CurrentAmount
    {
        get => _currentAmount;
        private set
        {
            _manaSlider.value = Mathf.Round(value);
            _manaText.text = $"{Mathf.Round(value)} / {_maxAmount}";

            _currentAmount = value;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        _maxAmount = PlayerStats.Instance.Mana;

        _manaSlider.minValue = 0;
        _manaSlider.maxValue = Mathf.Round(_maxAmount);

        CurrentAmount = _maxAmount;
    }

    private void Update()
    {
        CurrentAmount = Mathf.Clamp(CurrentAmount + _regenerationSpeed * Time.deltaTime, 0f, _maxAmount);
    }

    public bool TryDepleteMana(float amount)
    {
        if (CurrentAmount < amount)
            return false;

        CurrentAmount -= amount;
        return true;
    }

    public void UpdateMaxAmount(float newAmount)
    {
        float scaledCurrentAmount = CurrentAmount / _maxAmount * newAmount;

        _manaSlider.minValue = 0;
        _manaSlider.maxValue = Mathf.Round(newAmount);

        _maxAmount = newAmount;
        CurrentAmount = scaledCurrentAmount;
    }
}
