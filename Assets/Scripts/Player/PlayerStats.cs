using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public const int MaxUpgradeLevel = 3;

    private const string HealthKey = "player_health";
    private const string ManaKey = "player_mana";
    private const string IntellectKey = "player_intellect";

    private readonly float[] _healthValues = new float[1 + MaxUpgradeLevel] { 100f, 150f, 200f, 250f };
    private readonly float[] _manaValues = new float[1 + MaxUpgradeLevel] { 100f, 150f, 200f, 250f };
    private readonly float[] _intellectValues = new float[1 + MaxUpgradeLevel] { 100f, 150f, 200f, 250f };

    private readonly int[] _upgradePrices = new int[3] { 10, 20, 30 };

    public float Health { get; private set; }
    public float Mana { get; private set; }
    public float Intellect { get; private set; }

    public int HealthUpgradePrice => IsMaxHealth ? -1 : _upgradePrices[CurrentHealthValueIndex];
    public int ManaUpgradePrice => IsMaxMana ? -1 : _upgradePrices[CurrentManaValueIndex];
    public int IntellectUpgradePrice => IsMaxIntellect ? -1 : _upgradePrices[CurrentIntellectValueIndex];

    public int CurrentHealthValueIndex => Array.IndexOf(_healthValues, Health);
    public int CurrentManaValueIndex => Array.IndexOf(_manaValues, Mana);
    public int CurrentIntellectValueIndex => Array.IndexOf(_intellectValues, Intellect);

    private bool IsMaxHealth => Health == _healthValues[^1];
    private bool IsMaxMana => Mana == _manaValues[^1];
    private bool IsMaxIntellect => Intellect == _intellectValues[^1];

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        bool newGame = !PlayerPrefs.HasKey(HealthKey);

        if (newGame)
        {
            Health = _healthValues[0];
            Mana = _manaValues[0];
            Intellect = _intellectValues[0];

            Save();
        }
        else
        {
            Health = PlayerPrefs.GetFloat(HealthKey);
            Mana = PlayerPrefs.GetFloat(ManaKey);
            Intellect = PlayerPrefs.GetFloat(IntellectKey);
        }
    }

    public void UpgradeHealth()
    {
        int currentIndex = CurrentHealthValueIndex;

        if (currentIndex == _healthValues.Length - 1)
            return;

        if (currentIndex == -1)
        {
            Debug.LogError("Player health was outside of the range of possible values. Setting back to level 1.");

            Health = _healthValues[0];
            Save();

            return;
        }

        Health = _healthValues[currentIndex + 1];
        Save();

        PlayerHealthManager.Instance.UpdateMaxAmount(Health);
    }

    public void UpgradeMana()
    {
        int currentIndex = CurrentManaValueIndex;

        if (currentIndex == _manaValues.Length - 1)
            return;

        if (currentIndex == -1)
        {
            Debug.LogError("Player mana was outside of the range of possible values. Setting back to level 1.");

            Mana = _manaValues[0];
            Save();

            return;
        }

        Mana = _manaValues[currentIndex + 1];
        Save();

        ManaManager.Instance.UpdateMaxAmount(Mana);
    }

    public void UpgradeIntellect()
    {
        int currentIndex = CurrentIntellectValueIndex;

        if (currentIndex == _intellectValues.Length - 1)
            return;

        if (currentIndex == -1)
        {
            Debug.LogError("Player strength was outside of the range of possible values. Setting back to level 1.");

            Intellect = _intellectValues[0];
            Save();

            return;
        }

        Intellect = _intellectValues[currentIndex + 1];
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(HealthKey, Health);
        PlayerPrefs.SetFloat(ManaKey, Mana);
        PlayerPrefs.SetFloat(IntellectKey, Intellect);
        PlayerPrefs.Save();
    }
}
