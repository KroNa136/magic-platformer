using UnityEngine;

public class PoisonousMushroom : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(transform, _damage);
    }
}
