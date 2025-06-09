using UnityEngine;

public class AttackAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    [Space]

    [SerializeField] protected AudioClip _attackAudioClip;
    [SerializeField] protected AudioClip _attack2AudioClip;
    [SerializeField] protected AudioClip _attackFailAudioClip;

    public void Attack()
    {
        if (_audioSource != null && _attackAudioClip != null)
            _audioSource.PlayOneShot(_attackAudioClip);
    }

    public void Attack2()
    {
        if (_audioSource != null && _attack2AudioClip != null)
            _audioSource.PlayOneShot(_attack2AudioClip);
    }

    public void AttackFail()
    {
        if (_audioSource != null && _attackFailAudioClip != null)
            _audioSource.PlayOneShot(_attackFailAudioClip);
    }
}
