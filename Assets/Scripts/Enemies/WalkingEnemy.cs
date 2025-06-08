using UnityEngine;

[RequireComponent(typeof(WalkingMovement))]
public abstract class WalkingEnemy : Enemy
{
    protected WalkingMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<WalkingMovement>();

        OnAwake();
    }

    protected override void MoveTo(Vector2 position)
    {
        float xDifference = position.x - transform.position.x;

        _movement.HorizontalInput = xDifference switch
        {
            < 0f => -1f,
            > 0f => 1f,
            _ => 0f
        };
    }

    protected override void StayInPlace()
    {
        _movement.HorizontalInput = 0f;
        _movement.JumpInput = false;
    }

    protected abstract void OnAwake();
}
