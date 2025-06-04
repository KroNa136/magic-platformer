using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const float TouchDamage = 10f;

    public abstract void MoveTo(Vector3 position);
    public abstract void StayInPlace();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.collider;

        if (other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, TouchDamage);
    }
}
