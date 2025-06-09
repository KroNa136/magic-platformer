using System.Collections;
using UnityEngine;

public class GiantMushroom : WalkingEnemy
{
    [SerializeField][Min(0f)] private float _playerVisibilityDistance;
    [SerializeField][Min(0f)] private float _playerAttackDistance;

    [Space]

    [SerializeField][Min(0f)] private float _delayBeforeAttack;
    [SerializeField] private Vector2 _attackPositionOffset;
    [SerializeField][Min(0f)] private float _attackAreaRadius;
    [SerializeField][Min(0f)] private float _attackDamage;

    private Transform _player;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AttackAudioController _audioController;

    private bool _isAttacking = false;

    private Vector2 AttackPosition
    {
        get
        {
            Vector2 attackDirection = new
            (
                x: _spriteRenderer != null && (_spriteRenderer.flipX != _movement.SpriteIsFlippedByDefault) ? -1f : 1f,
                y: 0f
            );

            return (Vector2) transform.position + attackDirection * _attackPositionOffset.x + new Vector2(0f, _attackPositionOffset.y);
        }
    }

    protected override void OnAwake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        TryGetComponent(out _animator);
        TryGetComponent(out _audioController);
    }

    private void Start()
    {
        _player = PlayerMovement.Instance.transform;
    }

    private void Update()
    {
        bool isCompletelyOnGround = _movement.CheckGround(strict: true);
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (_isAttacking)
        {
            StayInPlace();
        }
        else if (distanceToPlayer > _playerVisibilityDistance)
        {
            StayInPlace();
        }
        else if (distanceToPlayer > _playerAttackDistance)
        {
            bool willBeCompletelyOnGroundInNextFrame = _movement.CheckGround(_movement.PredictNextPhysicsFramePosition(), strict: true);

            if (!isCompletelyOnGround || willBeCompletelyOnGroundInNextFrame)
                MoveTo(_player.position);
        }
        else if (isCompletelyOnGround)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        _audioController.Bind(audioController => audioController.Attack());

        if (_animator != null)
        {
            string randomAttackAnimation = Random.Range(0, 2) == 0 ? "Attack1" : "Attack2";

            _animator.SetTrigger(randomAttackAnimation);
            yield return null;

            float timer = 0f;
            bool attacked = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName(randomAttackAnimation))
            {
                timer += Time.deltaTime;

                if (timer > _delayBeforeAttack && !attacked)
                {
                    DoAttack();
                    attacked = true;
                }

                yield return null;
            }

            if (!attacked)
                DoAttack();
        }

        _isAttacking = false;
    }

    private void DoAttack()
    {
        var colliders = Physics2D.OverlapCircleAll(AttackPosition, _attackAreaRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(transform, _attackDamage);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, _attackAreaRadius);
    }
}
