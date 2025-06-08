using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private const string CurrentLevelKey = "current_level";

    public enum PortalType
    {
        Entrance,
        Exit
    }

    [SerializeField] private CanvasGroup _blackScreen;
    [SerializeField] private float _fadeDuration = 1f;

    [Space]

    [SerializeField] private PortalType _type;
    [SerializeField] private int _level;

    private bool _wasActivated = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") || _wasActivated)
            return;

        _wasActivated = true;

        if (_type == PortalType.Entrance)
            StartCoroutine(BlackScreenFadeIn());
        else
            StartCoroutine(BlackScreenFadeOut());
    }

    private IEnumerator BlackScreenFadeIn()
    {
        float time = 0f;

        while (_blackScreen.alpha < 1f)
        {
            _blackScreen.alpha = Mathf.Lerp(0f, 1f, time / _fadeDuration);

            time += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync($"Level {_level}", LoadSceneMode.Single);
    }

    private IEnumerator BlackScreenFadeOut()
    {
        float time = 0f;

        while (_blackScreen.alpha > 0f)
        {
            _blackScreen.alpha = Mathf.Lerp(1f, 0f, time / _fadeDuration);

            time += Time.deltaTime;
            yield return null;
        }

        PlayerPrefs.SetInt(CurrentLevelKey, _level);
        PlayerPrefs.Save();
    }
}
