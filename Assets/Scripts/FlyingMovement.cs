using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class FlyingMovement : Movement
{
    [SerializeField][Min(0f)] private float _horizontalMoveSpeed = 5f;
    [SerializeField][Min(0f)] private float _verticalMoveSpeed = 5f;

    private float _initialHorizontalMoveSpeed;
    private float _initialVerticalMoveSpeed;

    private Rigidbody2D _rigidbody;

    public float HorizontalInput { get; set; } = 0f;
    public float VerticalInput { get; set; } = 0f;

    protected override void OnAwake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _initialHorizontalMoveSpeed = _horizontalMoveSpeed;
        _initialVerticalMoveSpeed = _verticalMoveSpeed;
    }

    private void Update()
    {
        _animator.Bind(animator => animator.SetBool("Move", Mathf.Abs(HorizontalInput) > 0f || Mathf.Abs(VerticalInput) > 0f));
    }

    private void FixedUpdate()
    {
        Fly();
    }

    private void Fly()
    {
        if (!_canMove)
            return;

        _rigidbody.linearVelocityX = HorizontalInput * _horizontalMoveSpeed;

        if (HorizontalInput > 0f)
            _spriteRenderer.flipX = false;
        else if (HorizontalInput < 0f)
            _spriteRenderer.flipX = true;

        _rigidbody.linearVelocityY = VerticalInput * _verticalMoveSpeed;
    }

    public void ModifyMovementSpeed(float multiplier)
    {
        _horizontalMoveSpeed *= multiplier;
        _verticalMoveSpeed *= multiplier;
    }

    public void ResetMovementSpeed()
    {
        _horizontalMoveSpeed = _initialHorizontalMoveSpeed;
        _verticalMoveSpeed = _initialVerticalMoveSpeed;
    }
}
