using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class HealthManager : MonoBehaviour, IDamageable
{
    [HideInInspector] public UnityEvent<float> OnCurrentAmountChange = new();

    [SerializeField][Min(0f)] protected float _maxAmount = 100f;

    [Space]

    [SerializeField] private bool _destroyGameObjectAfterDeath = false;
    [SerializeField][Min(0f)] private float _fadeAfterDeathDuration = 2f;

    private float _currentAmount;
    public float CurrentAmount
    {
        get => _currentAmount;
        protected set
        {
            OnCurrentAmountChange.Invoke(value);
            _currentAmount = value;
        }
    }

    private SpriteRenderer _spriteRenderer;
    private Knockback _knockback;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        TryGetComponent(out _knockback);
        TryGetComponent(out _animator);

        OnAwake();
    }

    private void Start()
    {
        OnStart();

        CurrentAmount = _maxAmount;
    }

    public void TakeDamage(Transform source, float amount)
    {
        CurrentAmount = Mathf.Clamp(CurrentAmount - amount, 0f, _maxAmount);

        if (CurrentAmount > 0f)
        {
            //_animator.Bind(animator => animator.SetTrigger("TakeHit"));
            _knockback.Bind(knockback => knockback.Apply(source));
        }
        else
        {
            Die();
        }
    }

    public void TakeLethalDamage()
    {
        CurrentAmount = 0;
        Die();
    }

    protected void Die()
    {
        if (TryGetComponent(out Movement movement))
            movement.Disable();

        if (TryGetComponent(out Collider2D collider))
            Destroy(collider);

        if (TryGetComponent(out Rigidbody2D rigidbody))
        {
            rigidbody.linearVelocity = Vector2.zero;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        OnDeath();

        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).normalizedTime < 1f)
                yield return null;
        }

        if (_destroyGameObjectAfterDeath)
            yield return FadeOutAndDestroy();
    }

    private IEnumerator FadeOutAndDestroy()
    {
        Color color = _spriteRenderer.color;

        Light2D[] lights = GetComponentsInChildren<Light2D>();
        float[] initialLightIntensities = new float[lights.Length];

        for (int i = 0; i < lights.Length; i++)
            initialLightIntensities[i] = lights[i].intensity;

        float time = 0f;
        float interpolator;

        while (_spriteRenderer.color.a > 0f)
        {
            interpolator = time / _fadeAfterDeathDuration;

            _spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0f, interpolator));

            for (int i = 0; i < lights.Length; i++)
                lights[i].intensity = Mathf.Lerp(initialLightIntensities[i], 0f, interpolator);

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDeath();
}
