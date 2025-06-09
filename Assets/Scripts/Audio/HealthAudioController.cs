using UnityEngine;

public class HealthAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    [Space]

    [SerializeField] protected AudioClip _takeDamageAudioClip;
    [SerializeField] protected AudioClip _dieAudioClip;

    public void TakeDamage()
    {
        if (_audioSource != null && _takeDamageAudioClip != null)
            _audioSource.PlayOneShot(_takeDamageAudioClip);
    }

    public void Die()
    {
        if (_audioSource != null && _dieAudioClip != null)
            _audioSource.PlayOneShot(_dieAudioClip);
    }
}
