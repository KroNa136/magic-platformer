using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Transform source, float amount);
    public void TakeLethalDamage();
}
