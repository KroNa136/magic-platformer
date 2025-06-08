using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected const float TouchDamage = 10f;

    protected abstract void MoveTo(Vector2 position);
    protected abstract void StayInPlace();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.collider;

        if (other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, TouchDamage);
    }
}
