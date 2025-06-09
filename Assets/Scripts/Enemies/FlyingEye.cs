using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyHealthManager))]
public class FlyingEye : FlyingEnemy
{
    [SerializeField][Min(0f)] private float _playerVisibilityDistance;
    [SerializeField][Min(0f)] private float _playerAttackDistance;
    [SerializeField][Min(0f)] private float _attackOvershootMinDistance;
    [SerializeField][Min(0f)] private float _attackOvershootMaxDistance;
    [SerializeField][Min(0f)] private float _attackSpeedMultiplier;

    [Space]

    [SerializeField][Min(0f)] private float _attackDelay;
    [SerializeField][Min(0f)] private float _attackDamage;

    private Transform _player;

    private EnemyHealthManager _healthManager;
    private Animator _animator;
    private AttackAudioController _audioController;

    private bool _canAttack = true;
    private bool _isAttacking = false;
    private Vector2 _attackOvershootTargetPosition = Vector2.zero;

    protected override void OnAwake()
    {
        _healthManager = GetComponent<EnemyHealthManager>();
        _healthManager.OnCurrentAmountChange.AddListener(_ => StopAttacking());

        TryGetComponent(out _animator);
        TryGetComponent(out _audioController);
    }

    private void Start()
    {
        _player = PlayerMovement.Instance.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (_isAttacking)
        {
            MoveTo(_attackOvershootTargetPosition);

            bool isCloseEnough = Vector2.Distance(transform.position, _attackOvershootTargetPosition) < 0.1f;

            if (isCloseEnough)
                StopAttacking();
        }
        else if (distanceToPlayer > _playerVisibilityDistance)
        {
            StayInPlace();
        }
        else if (distanceToPlayer <= _playerAttackDistance && _canAttack)
        {
            Attack();
        }
        else
        {
            MoveTo(_player.position);
        }
    }

    private void Attack()
    {
        _isAttacking = true;

        _audioController.Bind(audioController => audioController.Attack());

        _animator.Bind(animator => _animator.SetTrigger("Attack"));

        Vector2 directionToPlayer = (_player.position - transform.position).normalized;
        _attackOvershootTargetPosition = (Vector2) _player.position + directionToPlayer * Random.Range(_attackOvershootMinDistance, _attackOvershootMaxDistance);

        _movement.ModifyMovementSpeed(_attackSpeedMultiplier);
    }

    private void StopAttacking()
    {
        _movement.ResetMovementSpeed();
        _isAttacking = false;

        _canAttack = false;
        StartCoroutine(WaitForAttackDelay());
    }

    private IEnumerator WaitForAttackDelay()
    {
        yield return new WaitForSeconds(_attackDelay);

        _canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, _isAttacking ? _attackDamage : TouchDamage);
    }
}
