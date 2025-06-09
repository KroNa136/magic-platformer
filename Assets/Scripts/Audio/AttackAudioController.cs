using UnityEngine;

public class AttackAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    [Space]

    [SerializeField] protected AudioClip _attackAudioClip;
    [SerializeField] protected AudioClip _attackFailAudioClip;

    public void Attack()
    {
        if (_audioSource != null && _attackAudioClip != null)
            _audioSource.PlayOneShot(_attackAudioClip);
    }

    public void AttackFail()
    {
        if (_audioSource != null && _attackFailAudioClip != null)
            _audioSource.PlayOneShot(_attackFailAudioClip);
    }
}
