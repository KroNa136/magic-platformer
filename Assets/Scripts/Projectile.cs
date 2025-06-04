using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _moveSpeed = 5f;
    [SerializeField][Min(0f)] private float _autoDestroyDelay = 10f;

    private float _damage;
    private Vector2 _moveDirection;
    private LayerMask _targetLayerMask;

    private Animator _animator;

    private bool _canMove = false;

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(_autoDestroyDelay));
    }

    public void Init(float damage, Vector2 moveDirection, LayerMask targetLayerMask)
    {
        _damage = damage;
        _moveDirection = moveDirection;
        _targetLayerMask = targetLayerMask;

        transform.localScale = new Vector3
        (
            x: _moveDirection.x < 0f ? -1f : 1f,
            y: 1f,
            z: 1f
        );

        _canMove = true;
    }

    private void Update()
    {
        if (_canMove)
            transform.Translate(_moveDirection.normalized * (_moveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.IsNotInLayerMask(_targetLayerMask))
            return;

        if (collider.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, _damage);

        StopCoroutine(nameof(DestroyAfterDelay));
        StartCoroutine(StopAndDestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return StopAndDestroyAfterAnimation();
    }

    private IEnumerator StopAndDestroyAfterAnimation()
    {
        _canMove = false;

        if (_animator != null)
        {
            _animator.SetTrigger("Destroy");
            yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Projectile_Explode"))
                yield return null;
        }

        Destroy(gameObject);
    }
}
