using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _boss;

    private bool _wasActivated = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") || _wasActivated)
            return;

        _wasActivated = true;
        _boss.SetActive(true);
    }
}
