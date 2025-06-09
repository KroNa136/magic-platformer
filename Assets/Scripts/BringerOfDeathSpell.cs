using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BringerOfDeathSpell : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _fadeDuration;
    [SerializeField] private Light2D _light;
    [SerializeField][Min(0f)] private float _lightIntensity;

    [SerializeField][Min(0f)] private float _delayBeforeAttack;
    [SerializeField][Min(0f)] private float _attackDuration;
    [SerializeField] private Collider2D _attackTrigger;

    private float _attackDamage;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private SpellAudioController _audioController;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        TryGetComponent(out _animator);
        TryGetComponent(out _audioController);

        _attackTrigger.enabled = false;
    }

    public void Init(float damage)
    {
        _attackDamage = damage;

        StartCoroutine(FadeInThenAttackThenFadeOutAndDestroy());
    }

    private IEnumerator FadeInThenAttackThenFadeOutAndDestroy()
    {
        yield return FadeIn();
        yield return Attack();
        yield return FadeOutAndDestroy();
    }

    private IEnumerator FadeIn()
    {
        Color color = _spriteRenderer.color;

        float time = 0f;
        float interpolator;

        while (_spriteRenderer.color.a < 1f)
        {
            interpolator = time / _fadeDuration;

            _spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0f, 1f, interpolator));
            _light.intensity = Mathf.Lerp(0f, _lightIntensity, interpolator);

            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        _audioController.Bind(audioController => audioController.Hit());

        if (_animator != null)
        {
            _animator.SetTrigger("Cast");
            yield return null;

            float timer = 0f;
            bool attacked = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Spell"))
            {
                timer += Time.deltaTime;

                if (timer > _delayBeforeAttack && !attacked)
                {
                    yield return DoAttack();
                    attacked = true;
                }

                yield return null;
            }

            if (!attacked)
                yield return DoAttack();
        }
        else
        {
            yield return DoAttack();
        }
    }

    private IEnumerator DoAttack()
    {
        _attackTrigger.enabled = true;

        yield return new WaitForSeconds(_attackDuration);

        _attackTrigger.enabled = false;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        Color color = _spriteRenderer.color;

        float time = 0f;
        float interpolator;

        while (_spriteRenderer.color.a > 0f)
        {
            interpolator = time / _fadeDuration;

            _spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, interpolator));
            _light.intensity = Mathf.Lerp(_lightIntensity, 0f, interpolator);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, _attackDamage);
    }
}
