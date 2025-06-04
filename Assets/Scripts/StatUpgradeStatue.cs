using UnityEngine;

public class StatUpgradeStatue : MonoBehaviour
{
    [SerializeField] private StatsModal _statsModal;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            _statsModal.Activate();
    }
}
