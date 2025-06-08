using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _projectileSpawnPointHorizontalOffset;
    [SerializeField] private LayerMask _projectileTargetLayerMask;

    [Space]

    [SerializeField][Min(0f)] private float _damage = 50f;
    [SerializeField][Min(0f)] private float _manaConsumption = 20f;
    [SerializeField][Min(0f)] private float _delayBeforeProjectileSpawn = 1f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _canAttack = true;
    private bool _isAttacking = false;

    private float Damage => _damage * (PlayerStats.Instance.Intellect * 0.01f);

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        TryGetComponent(out _animator);
    }

    private void Update()
    {
        if (InputManager.Instance.Attack && !_isAttacking && _canAttack && ManaManager.Instance.TryDepleteMana(_manaConsumption))
            StartCoroutine(PlayAnimationAndSpawnProjectile());
    }

    private IEnumerator PlayAnimationAndSpawnProjectile()
    {
        _isAttacking = true;

        if (_animator != null)
        {
            PlayerMovement.Instance.DisableMovement();

            _animator.SetTrigger("Attack");
            yield return null;

            float timer = 0f;
            bool spawnedProjectile = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Attack"))
            {
                timer += Time.deltaTime;

                if (timer > _delayBeforeProjectileSpawn && !spawnedProjectile)
                {
                    SpawnProjectile();
                    spawnedProjectile = true;
                }

                yield return null;
            }

            if (!spawnedProjectile)
                SpawnProjectile();

            PlayerMovement.Instance.EnableMovement();
        }

        _isAttacking = false;
    }

    private void SpawnProjectile()
    {
        Vector2 moveDirection = new
        (
            x: _spriteRenderer.flipX ? -1f : 1f,
            y: 0f
        );

        GameObject projectile = Instantiate
        (
            original: _projectilePrefab,
            position: _projectileSpawnPoint.position + new Vector3(_projectileSpawnPointHorizontalOffset * moveDirection.x, 0f, 0f),
            rotation: Quaternion.identity
        );

        if (projectile.TryGetComponent<Projectile>(out var projectileComponent))
        {
            projectileComponent.Init
            (
                damage: Damage,
                moveDirection: moveDirection,
                targetLayerMask: _projectileTargetLayerMask
            );
        }
    }

    public void EnableAttack() => _canAttack = true;
    public void DisableAttack() => _canAttack = false;
}
