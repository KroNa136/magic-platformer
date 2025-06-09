using UnityEngine;

public class SpellAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    [Space]

    [SerializeField] protected AudioClip _hitAudioClip;

    public void Hit()
    {
        if (_audioSource != null && _hitAudioClip != null)
            _audioSource.PlayOneShot(_hitAudioClip);
    }
}
