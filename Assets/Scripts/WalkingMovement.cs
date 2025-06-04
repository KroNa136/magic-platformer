using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class WalkingMovement : Movement
{
    private const float GroundCheckRayDistance = 0.1f;

    [SerializeField][Min(0f)] private float _moveSpeed = 5f;
    [SerializeField][Min(0f)] private float _jumpForce = 5f;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    public bool IsGrounded { get; private set; } = false;

    public float HorizontalInput { get; set; } = 0f;
    public bool JumpInput { get; set; } = false;

    protected override void OnAwake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        TryGetComponent(out _boxCollider);
    }

    private void Update()
    {
        /*
        if (CheckGround() && !_isGrounded)
            _audioController.Land();
        */

        IsGrounded = CheckGround();

        if (JumpInput && IsGrounded)
            Jump();

        _animator.Bind(animator =>
        {
            animator.SetBool("Move", Mathf.Abs(HorizontalInput) > 0f);

            /*
            if (_rigidbody.linearVelocityY < 0f)
                animator.SetTrigger("Fall");
            */
        });
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if (!_canMove)
            return;

        _rigidbody.linearVelocityX = HorizontalInput * _moveSpeed;

        if (HorizontalInput > 0f)
            _spriteRenderer.flipX = false;
        else if (HorizontalInput < -0f)
            _spriteRenderer.flipX = true;

        //_audioController.SetWalking(_rigidbody.linearVelocityX != 0 && CheckGround());
    }

    private void Jump()
    {
        if (!_canMove)
            return;

        _rigidbody.linearVelocityY = 0f;
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        _animator.Bind(animator => animator.SetTrigger("Jump"));
        //_audioController.Jump();
    }

    public bool CheckGround(bool strict = false)
        => CheckGround(transform.position, strict);

    public bool CheckGround(Vector2 checkPosition, bool strict = false)
    {
        if (_boxCollider == null)
            return false;

        Vector2 colliderCenter = checkPosition + _boxCollider.offset;

        Vector2[] rayOriginPositions = new Vector2[3]
        {
            colliderCenter - _boxCollider.size / 2f, // левый нижний угол
            new(colliderCenter.x, colliderCenter.y - _boxCollider.size.y / 2f), // центр снизу
            new(colliderCenter.x + _boxCollider.size.x / 2f, colliderCenter.y - _boxCollider.size.y / 2f), // правый нижний угол
        };

        return strict ?
            CheckIfIsCompletelyOnGround(rayOriginPositions) :
            CheckIfIsPartiallyOnGround(rayOriginPositions);
    }

    private bool CheckIfIsCompletelyOnGround(Vector2[] rayOriginPositions)
    {
        foreach (Vector2 position in rayOriginPositions)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, -transform.up, GroundCheckRayDistance, _groundLayerMask);

            if (hit.collider == null)
                return false;
        }

        return true;
    }

    private bool CheckIfIsPartiallyOnGround(Vector2[] rayOriginPositions)
    {
        foreach (Vector2 position in rayOriginPositions)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, -transform.up, GroundCheckRayDistance, _groundLayerMask);

            if (hit.collider != null)
                return true;
        }

        return false;
    }

    public Vector2 PredictNextPhysicsFramePosition()
        => (Vector2) transform.position + _rigidbody.linearVelocity * Time.fixedDeltaTime;
}
