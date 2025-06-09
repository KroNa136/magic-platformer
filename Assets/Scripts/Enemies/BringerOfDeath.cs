using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BringerOfDeathHealthManager))]
public class BringerOfDeath : WalkingEnemy
{
    [SerializeField][Min(0f)] private float _delayAfterAppearance;

    [Space]

    [SerializeField][Min(0f)] private float _playerAttackDistance;

    [Space]

    [SerializeField][Min(0f)] private float _delayBeforeAttack;
    [SerializeField] private Vector2 _attackPositionOffset;
    [SerializeField][Min(0f)] private float _attackAreaRadius;
    [SerializeField][Min(0f)] private float _attackDamage;

    [Space]

    [SerializeField] private GameObject _spellPrefab;
    [SerializeField][Min(0f)] private float _delayBeforeCast;
    [SerializeField][Min(0f)] private float _castMinDelay;
    [SerializeField][Min(0f)] private float _castMaxDelay;
    [SerializeField][Min(0f)] private float _spellHorizontalOffsetFromPlayer;
    [SerializeField][Min(0f)] private float _spellVerticalOffset;
    [SerializeField][Min(0f)] private float _spellDamage;

    [Space]

    [SerializeField][Min(0f)] private float _delayBeforeTeleport;
    [SerializeField][Min(0f)] private float _teleportMinDelay;
    [SerializeField][Min(0f)] private float _teleportMaxDelay;
    [SerializeField] private float _teleportAreaMinX;
    [SerializeField] private float _teleportAreaMaxX;
    [SerializeField][Min(0f)] private float _teleportMinDistanceFromPlayer;

    private Transform _player;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private BringerOfDeathAttackAudioController _audioController;
    private BringerOfDeathHealthManager _healthManager;

    private bool _isAppearing = true;
    private bool _isAttacking = false;
    private bool _canCast = false;
    private bool _isCasting = false;
    private bool _canTeleport = false;
    private bool _isTeleporting = false;

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
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthManager = GetComponent<BringerOfDeathHealthManager>();

        TryGetComponent(out _animator);
        TryGetComponent(out _audioController);
    }

    private void Start()
    {
        _player = PlayerMovement.Instance.transform;

        StartCoroutine(WaitForAppearing());
        StartCoroutine(CastTimer());
        StartCoroutine(TeleportTimer());
    }

    private void Update()
    {
        bool isCompletelyOnGround = _movement.CheckGround(strict: true);
        float horizontalDistanceToPlayer = Mathf.Abs(transform.position.x - _player.position.x);

        if (_isAppearing)
        {
            _canCast = false;
            _canTeleport = false;

            StayInPlace();
        }
        else if (_isAttacking)
        {
            _canCast = false;
            _canTeleport = true;

            StayInPlace();
        }
        else if (_isCasting)
        {
            _canCast = false;
            _canTeleport = false;

            StayInPlace();
        }
        else if (_isTeleporting)
        {
            _canCast = false;
            _canTeleport = false;

            StayInPlace();
        }
        else if (horizontalDistanceToPlayer > _playerAttackDistance)
        {
            _canCast = true;
            _canTeleport = true;

            bool willBeCompletelyOnGroundInNextFrame = _movement.CheckGround(_movement.PredictNextPhysicsFramePosition(), strict: true);

            if (!isCompletelyOnGround || willBeCompletelyOnGroundInNextFrame)
                MoveTo(_player.position);
        }
        else
        {
            _canCast = false;
            _canTeleport = false;

            StartCoroutine(Attack());
        }
    }

    private IEnumerator WaitForAppearing()
    {
        if (_animator != null)
        {
            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Appear"))
                yield return null;
        }

        yield return new WaitForSeconds(_delayAfterAppearance);

        _healthManager.ActivateBossBar();
        _isAppearing = false;
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;

        _audioController.Bind(audioController => audioController.Attack());

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
            yield return null;

            float timer = 0f;
            bool attacked = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Attack"))
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
        else
        {
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

    private IEnumerator CastTimer()
    {
        while (true)
        {
            float castDelay = Random.Range(_castMinDelay, _castMaxDelay);

            float timer = 0f;

            while (timer < castDelay)
            {
                if (_canCast)
                    timer += Time.deltaTime;

                yield return null;
            }

            while (_isAttacking || _isTeleporting)
                yield return null;

            yield return Cast();
        }
    }

    private IEnumerator Cast()
    {
        _isCasting = true;

        _audioController.Bind(audioController => audioController.Cast());

        if (_animator != null)
        {
            _animator.SetTrigger("Cast");
            yield return null;

            float timer = 0f;
            bool casted = false;

            while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Cast"))
            {
                timer += Time.deltaTime;

                if (timer > _delayBeforeCast && !casted)
                {
                    SpawnSpells();
                    casted = true;
                }

                yield return null;
            }

            if (!casted)
                SpawnSpells();
        }
        else
        {
            SpawnSpells();
        }

        _isCasting = false;
    }

    private void SpawnSpells()
    {
        float horizontalDistanceToPlayer = Mathf.Abs(transform.position.x - _player.position.x);
        int spellCount = (horizontalDistanceToPlayer < _spellHorizontalOffsetFromPlayer * 4f) ? Random.Range(1, 3) : 1;

        float x = _player.position.x;
        float y = transform.position.y + _spellVerticalOffset;

        if (spellCount == 1)
        {
            SpawnSpell(new Vector2(x, y));
        }
        else
        {
            SpawnSpell(new Vector2(x - _spellHorizontalOffsetFromPlayer, y));
            SpawnSpell(new Vector2(x + _spellHorizontalOffsetFromPlayer, y));
        }
    }

    private void SpawnSpell(Vector2 position)
    {
        GameObject spell = Instantiate
        (
            original: _spellPrefab,
            position: position,
            rotation: Quaternion.identity
        );

        if (spell.TryGetComponent<BringerOfDeathSpell>(out var spellComponent))
            spellComponent.Init(damage: _spellDamage);
    }

    private IEnumerator TeleportTimer()
    {
        while (true)
        {
            float teleportDelay = Random.Range(_teleportMinDelay, _teleportMaxDelay);

            float timer = 0f;

            while (timer < teleportDelay)
            {
                if (_canTeleport)
                    timer += Time.deltaTime;

                yield return null;
            }

            while (_isAttacking || _isCasting)
                yield return null;

            yield return Teleport();
        }
    }

    private IEnumerator Teleport()
    {
        _isTeleporting = true;

        Vector2 targetPosition = new(0, transform.position.y);

        bool playerIsInTeleportArea = _player.position.x >= _teleportAreaMinX && _player.position.x <= _teleportAreaMaxX;

        if (playerIsInTeleportArea)
        {
            float teleportFirstAreaRightPoint = _player.position.x - _teleportMinDistanceFromPlayer;
            float teleportSecondAreaLeftPoint = _player.position.x + _teleportMinDistanceFromPlayer;

            bool chooseFirstArea = Random.Range(0, 2) == 0;

            targetPosition.x = chooseFirstArea ?
                Random.Range(_teleportAreaMinX, teleportFirstAreaRightPoint) :
                Random.Range(teleportSecondAreaLeftPoint, _teleportAreaMaxX);
        }
        else
        {
            targetPosition.x = Random.Range(_teleportAreaMinX, _teleportAreaMaxX);
        }

        RaycastHit2D groundHitAtCurrentPosition = Physics2D.Raycast
        (
            origin: new Vector2(transform.position.x, transform.position.y),
            direction: Vector2.down,
            distance: float.PositiveInfinity,
            layerMask: _movement.GroundLayerMask
        );

        RaycastHit2D groundHitAtTargetPosition = Physics2D.Raycast
        (
            origin: new Vector2(targetPosition.x, transform.position.y + 1f),
            direction: Vector2.down,
            distance: float.PositiveInfinity,
            layerMask: _movement.GroundLayerMask
        );

        if (groundHitAtTargetPosition.collider != null)
        {
            targetPosition.y += groundHitAtTargetPosition.point.y - groundHitAtCurrentPosition.point.y;

            if (_animator != null)
            {
                _audioController.Bind(audioController => audioController.TeleportStart());

                _animator.SetTrigger("Disappear");
                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;

                _collider.enabled = false;
                yield return new WaitForFixedUpdate();

                while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).normalizedTime < 1f)
                    yield return null;

                _spriteRenderer.enabled = false;

                yield return new WaitForSeconds(_delayBeforeTeleport);
                _rigidbody.MovePosition(targetPosition);
                yield return new WaitForFixedUpdate();

                _audioController.Bind(audioController => audioController.TeleportEnd());

                _spriteRenderer.enabled = true;

                _animator.SetTrigger("Reappear");
                yield return null;

                while (_animator.GetCurrentAnimatorStateInfo(layerIndex: 0).IsName("Reappear"))
                    yield return null;

                _collider.enabled = true;
                yield return new WaitForFixedUpdate();

                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;

                _collider.enabled = false;
                yield return new WaitForFixedUpdate();

                _rigidbody.MovePosition(targetPosition);

                _audioController.Bind(audioController => audioController.TeleportEnd());

                _collider.enabled = true;
                yield return new WaitForFixedUpdate();

                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        _isTeleporting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition, _attackAreaRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawLine(new Vector3(_teleportAreaMinX, transform.position.y + 2f), new Vector3(_teleportAreaMinX, transform.position.y - 2f));
        Gizmos.DrawLine(new Vector3(_teleportAreaMaxX, transform.position.y + 2f), new Vector3(_teleportAreaMaxX, transform.position.y - 2f));
    }
}
