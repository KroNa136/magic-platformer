using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;

    protected bool _canMove = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        TryGetComponent(out _animator);

        OnAwake();
    }

    protected abstract void OnAwake();

    public void Enable() => _canMove = true;
    public void Disable() => _canMove = false;
}
