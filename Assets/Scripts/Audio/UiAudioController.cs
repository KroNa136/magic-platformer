using UnityEngine;

public class UiAudioController : MonoBehaviour
{
    public static UiAudioController Instance;

    [SerializeField] private AudioSource _audioSource;

    [Space]

    [SerializeField] private AudioClip _pressButtonAudioClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    public void PressButton()
    {
        if (_audioSource != null && _pressButtonAudioClip != null)
            _audioSource.PlayOneShot(_pressButtonAudioClip);
    }
}
