using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _amount;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.TryGetComponent<IHealable>(out var healable))
        {
            healable.Heal(_amount);
            PlayerAudioController.Instance.Bind(audioController => audioController.PickHealthPotion());

            Destroy(gameObject);
        }
    }
}
