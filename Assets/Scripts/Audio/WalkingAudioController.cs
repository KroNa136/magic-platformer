using System.Collections;
using UnityEngine;

public class WalkingAudioController : MonoBehaviour
{
    [SerializeField] protected AudioSource _stepsAudioSource;
    [SerializeField] protected AudioSource _othersAudioSource;

    [Space]

    [SerializeField] protected AudioClip[] _stepAudioClips;
    [SerializeField] protected AudioClip _jumpAudioClip;
    [SerializeField] protected AudioClip _landAudioClip;

    [Space]

    [SerializeField][Min(0f)] private float _stepDuration = 0.3f;

    private void Start()
    {
        _stepsAudioSource.volume = 0f;

        StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        while (true)
        {
            if (_stepsAudioSource != null && _stepAudioClips.Length > 0)
            {
                AudioClip randomStepAudioClip = _stepAudioClips[Random.Range(0, _stepAudioClips.Length)];

                if (randomStepAudioClip != null )
                    _stepsAudioSource.PlayOneShot(randomStepAudioClip);
            }

            yield return new WaitForSeconds(_stepDuration);
        }
    }

    public void SetWalking(bool isWalking)
    {
        if (_stepsAudioSource != null)
            _stepsAudioSource.volume = isWalking ? _othersAudioSource.volume : 0f;
    }

    public void Jump()
    {
        if (_othersAudioSource != null && _jumpAudioClip != null)
            _othersAudioSource.PlayOneShot(_jumpAudioClip);
    }

    public void Land()
    {
        if (_othersAudioSource != null && _landAudioClip != null)
            _othersAudioSource.PlayOneShot(_landAudioClip);
    }
}
