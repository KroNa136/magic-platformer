using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [SerializeField] private float _force = 2f;
    [SerializeField] private float _angle = 45f;

    [Space]

    [SerializeField] private float _duration = 0.5f;

    private Rigidbody2D _rigidbody;
    private Movement _movement;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
    }

    public void Apply(Transform source)
        => StartCoroutine(Apply(transform.position - source.position));

    private IEnumerator Apply(Vector2 direction)
    {
        _movement.Disable();

        direction.y += Mathf.Sin(Mathf.Deg2Rad * _angle);
        direction.Normalize();

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.AddForce(direction * _force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_duration);

        _rigidbody.linearVelocity = Vector2.zero;

        _movement.Enable();
    }
}
