using UnityEngine;

public class BringerOfDeathAttackAudioController : AttackAudioController
{
    [SerializeField] protected AudioClip _castAudioClip;
    [SerializeField] protected AudioClip _teleportStartAudioClip;
    [SerializeField] protected AudioClip _teleportEndAudioClip;

    public void Cast()
    {
        if (_audioSource != null && _castAudioClip != null)
            _audioSource.PlayOneShot(_castAudioClip);
    }

    public void TeleportStart()
    {
        if (_audioSource != null && _teleportStartAudioClip != null)
            _audioSource.PlayOneShot(_teleportStartAudioClip);
    }

    public void TeleportEnd()
    {
        if (_audioSource != null && _teleportEndAudioClip != null)
            _audioSource.PlayOneShot(_teleportEndAudioClip);
    }
}
