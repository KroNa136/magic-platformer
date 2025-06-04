using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField][Min(0f)] private float _onSpawnImpulse = 3f;
    [SerializeField] private LayerMask _groundLayerMask;

    [Space]

    [SerializeField] private GameObject _parent;

    private void Start()
    {
        if (_parent != null && _parent.TryGetComponent(out Rigidbody2D rigidbody))
            AddImpulse(rigidbody);
    }

    private void AddImpulse(Rigidbody2D rigidbody)
    {
        float angle = Random.Range(-45f, 45f);
        Quaternion directionRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 direction = directionRotation * Vector2.up;

        rigidbody.AddForce(direction * _onSpawnImpulse, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin();
            Destroy(_parent);
        }
    }
}
