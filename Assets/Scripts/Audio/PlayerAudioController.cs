using System.Collections;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public static PlayerAudioController Instance;

    [SerializeField] protected AudioSource _audioSource;

    [Space]

    [SerializeField] private AudioClip _pickCoinAudioClip;
    [SerializeField] private AudioClip _spendCoinsAudioClip;
    [SerializeField] private AudioClip _pickHealthPotionAudioClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void PickCoin()
    {
        if (_audioSource != null && _pickCoinAudioClip != null)
            _audioSource.PlayOneShot(_pickCoinAudioClip);
    }

    public void SpendCoins()
    {
        if (_audioSource != null && _spendCoinsAudioClip != null)
            _audioSource.PlayOneShot(_spendCoinsAudioClip);
    }

    public void PickHealthPotion()
    {
        if (_audioSource != null && _pickHealthPotionAudioClip != null)
            _audioSource.PlayOneShot(_pickHealthPotionAudioClip);
    }
}
