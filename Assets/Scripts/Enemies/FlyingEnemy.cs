using UnityEngine;

[RequireComponent(typeof(FlyingMovement))]
public abstract class FlyingEnemy : Enemy
{
    protected FlyingMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<FlyingMovement>();

        OnAwake();
    }

    protected override void MoveTo(Vector2 position)
    {
        Vector2 direction = (position - (Vector2) transform.position).normalized;

        _movement.HorizontalInput = direction.x;
        _movement.VerticalInput = direction.y;
    }

    protected override void StayInPlace()
    {
        _movement.HorizontalInput = 0f;
        _movement.VerticalInput = 0f;
    }

    protected abstract void OnAwake();
}
