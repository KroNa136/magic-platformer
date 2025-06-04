using UnityEngine;

[RequireComponent(typeof(WalkingMovement), typeof(PlayerAttack))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    private WalkingMovement _movement;
    private PlayerAttack _playerAttack;

    public Vector2 MoveDirection { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        _movement = GetComponent<WalkingMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        float horizontalInput = InputManager.Instance.Horizontal;
        MoveDirection = new Vector2(horizontalInput, 0).normalized;

        _movement.HorizontalInput = horizontalInput;
        _movement.JumpInput = InputManager.Instance.Jump;

        if (_movement.IsGrounded)
            _playerAttack.EnableAttack();
        else
            _playerAttack.DisableAttack();
    }

    public void EnableMovement() => _movement.Enable();
    public void DisableMovement() => _movement.Disable();
}
