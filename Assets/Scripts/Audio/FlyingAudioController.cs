using System.Collections;
using UnityEngine;

public class FlyingAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField][Min(0f)] private float _volume;

    [Space]

    [SerializeField] protected AudioClip _flyAudioClip;

    private void Start()
    {
        if (_audioSource != null && _flyAudioClip != null)
        {
            _audioSource.volume = 0f;
            _audioSource.loop = true;
            _audioSource.clip = _flyAudioClip;
            _audioSource.Play();
        }
    }

    public void SetFlying(bool isFlying)
    {
        if (_audioSource != null)
            _audioSource.volume = isFlying ? _volume : 0f;
    }
}
