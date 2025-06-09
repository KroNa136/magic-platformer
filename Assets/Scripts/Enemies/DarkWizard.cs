using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizard : WalkingEnemy
{
    [SerializeField][Min(0f)] private float _playerVisibilityDistance;
    [SerializeField][Min(0f)] private float _playerAttackDistance;
    [SerializeField][Min(0f)] private float _runAwayMinDistance;
    [SerializeField][Min(0f)] private float _runAwayMaxDistance;

    [Space]

    [SerializeField][Min(0f)] private float _delayBeforeAttack1;
    [SerializeField][Min(0f)] private float _delayBeforeAttack2;
    [SerializeField] private Vector2 _attackPositionOffset;
    [SerializeField][Min(0f)] private float _attackAreaRadius;
    [SerializeField][Min(0f)] private float _attackDamage;

    private Transform _player;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AttackAudioController _audioController;

    private bool _isAttacking = false;
    private bool _isRunningAway = false;
    private Vector2 _runAwayPosition = Vector2.zero;

    private readonly List<Vector2> _previousFramePositions = new();
    private readonly float _maxPreviousFramePositions = 5;

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
        float horizontalDistanceToPlayer = Mathf.Abs(transform.position.x - _player.position.x);

        if (_isAttacking)
        {
            StayInPlace();
        }
        else if (_isRunningAway)
        {
            MoveTo(_runAwayPosition);

            bool isCloseEnough = Mathf.Abs(transform.position.x - _runAwayPosition.x) < 0.1f;
            bool isRunningInPlace = AllRecordedPositionsHaveXEqualTo(transform.position.x);
            bool isOvertakenByPlayer = Mathf.Sign(transform.position.x - _player.position.x) == Mathf.Sign(transform.position.x - _runAwayPosition.x);

            if (isCloseEnough || isRunningInPlace || isOvertakenByPlayer)
                _isRunningAway = false;
        }
        else if (distanceToPlayer > _playerVisibilityDistance)
        {
            StayInPlace();
        }
        else if (horizontalDistanceToPlayer > _playerAttackDistance)
        {
            bool willBeCompletelyOnGroundInNextFrame = _movement.CheckGround(_movement.PredictNextPhysicsFramePosition(), strict: true);

            if (!isCompletelyOnGround || willBeCompletelyOnGroundInNextFrame)
                MoveTo(_player.position);
        }
        else
        {
            StartCoroutine(AttackAndRunAway());
        }

        RecordCurrentPosition();
    }

    private void RecordCurrentPosition()
    {
        if (_previousFramePositions.Count == _maxPreviousFramePositions)
            _previousFramePositions.RemoveAt(0);

        _previousFramePositions.Add(transform.position);
    }

    private bool AllRecordedPositionsHaveXEqualTo(float x)
    {
        if (_previousFramePositions.Count == 0)
            return false;

        foreach (var position in _previousFramePositions)
        {
            if (position.x != x)
                return false;
        }

        return true;
    }

    private IEnumerator AttackAndRunAway()
    {
        _isAttacking = true;

        int randomAttackNumber = Random.Range(0, 2) == 0 ? 1 : 2;

        if (randomAttackNumber == 1)
            _audioController.Bind(audioController => audioController.Attack());
        else
            _audioController.Bind(audioController => audioController.Attack2());

        if (_animator != null)
        {
            string randomAttackAnimation = $"Attack{randomAttackNumber}";
            float delayBeforeAttack = randomAttackNumber == 0 ? _delayBeforeAttack1 : _delayBeforeAttack2;

            _animator.SetTrigger(randomAttackAnimation);
            yield return null;

            float timer = 0f;
            bool attacked = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName(randomAttackAnimation))
            {
                timer += Time.deltaTime;

                if (timer > delayBeforeAttack && !attacked)
                {
                    DoAttack();
                    attacked = true;
                }

                yield return null;
            }

            if (!attacked)
                DoAttack();
        }
        else
        {
            DoAttack();
        }

        _isAttacking = false;

        StartRunningAway();
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

    private void StartRunningAway()
    {
        float xDifference = transform.position.x - _player.position.x;

        float xDirectionAwayFromPlayer = xDifference switch
        {
            < 0f => -1f,
            > 0f => 1f,
            _ => 0f
        };

        bool decidedOnRunAwayPosition = false;

        float runAwayDistance = Random.Range(_runAwayMinDistance, _runAwayMaxDistance);

        do
        {
            _runAwayPosition.x = transform.position.x + xDirectionAwayFromPlayer * runAwayDistance;

            RaycastHit2D groundHitAtRunAwayPosition = Physics2D.Raycast
            (
                origin: new Vector2(_runAwayPosition.x, transform.position.y + 1f),
                direction: Vector2.down,
                distance: float.PositiveInfinity,
                layerMask: _movement.GroundLayerMask
            );

            if (groundHitAtRunAwayPosition.collider != null)
            {
                _runAwayPosition.y = groundHitAtRunAwayPosition.point.y;
                decidedOnRunAwayPosition = true;
            }

            runAwayDistance -= 0.1f;

            if (runAwayDistance < 0f)
            {
                _runAwayPosition.y = transform.position.y;
                decidedOnRunAwayPosition = true;
            }
        }
        while (!decidedOnRunAwayPosition);

        _isRunningAway = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, _attackAreaRadius);
    }
}
