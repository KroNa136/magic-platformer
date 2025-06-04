using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _amount;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<IHealable>(out var healable))
        {
            healable.Heal(_amount);
            Destroy(gameObject);
        }
    }
}
