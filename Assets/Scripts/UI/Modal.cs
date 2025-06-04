using System.Collections;
using UnityEngine;

public abstract class Modal : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeTime = 0.5f;

    public void Activate()
    {
        Time.timeScale = 0f;

        OnActivate();

        StopCoroutine(FadeOut());
        StartCoroutine(FadeIn());
    }

    public void Deactivate()
    {
        Time.timeScale = 1f;

        OnDeactivate();

        StopCoroutine(FadeIn());
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        float time = 0f;

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / _fadeTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float time = 0f;

        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / _fadeTime);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
}
