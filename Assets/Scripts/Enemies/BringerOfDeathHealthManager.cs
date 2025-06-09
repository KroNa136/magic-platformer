using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BringerOfDeathHealthManager : EnemyHealthManager
{
    [SerializeField] private CanvasGroup _bossBar;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthText;

    [Space]

    [SerializeField] private GameFinishedModal _gameFinishedModal;
    [SerializeField][Min(0f)] private float _delayBeforeShowingGameFinishedModal;

    [Space]

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioClip _bossMusicAudioClip;
    private AudioClip _defaultMusicAudioClip;

    protected override void OnStart()
    {
        _healthSlider.minValue = 0f;
        _healthSlider.maxValue = _maxAmount;
        _healthSlider.value = _maxAmount;

        if (_musicAudioSource != null && _bossMusicAudioClip != null)
        {
            _defaultMusicAudioClip = _musicAudioSource.clip;
            _musicAudioSource.Stop();
            _musicAudioSource.clip = _bossMusicAudioClip;
            _musicAudioSource.Play();
        }

        OnCurrentAmountChange.AddListener(newAmount =>
        {
            _healthSlider.value = Mathf.Round(newAmount);
            _healthText.text = $"{Mathf.Round(newAmount)} / {_maxAmount}";
        });
    }

    public void ActivateBossBar()
        => _bossBar.alpha = 1f;

    protected override void OnDeath()
    {
        if (TryGetComponent(out Enemy enemy))
            Destroy(enemy);

        _bossBar.alpha = 0f;

        if (_musicAudioSource != null && _bossMusicAudioClip != null)
        {
            _musicAudioSource.Stop();

            if (_defaultMusicAudioClip != null)
            {
                _musicAudioSource.clip = _defaultMusicAudioClip;
                _musicAudioSource.Play();
            }
        }

        StartCoroutine(ShowGameFinishedModalAfterDelay());
    }

    private IEnumerator ShowGameFinishedModalAfterDelay()
    {
        yield return new WaitForSeconds(_delayBeforeShowingGameFinishedModal);
        _gameFinishedModal.Activate();
    }
}
