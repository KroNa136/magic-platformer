using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    [Space]

    [SerializeField] private GameObject _coinPrefab;
    [SerializeField][Min(0f)] private int _onDeathMinCoinsCount;
    [SerializeField][Min(0f)] private int _onDeathMaxCoinsCount;
    [SerializeField][Min(0f)] private float _coinSpawnPositionHorizontalOffset = 0.1f;

    protected override void OnAwake()
    {
        
    }

    protected override void OnStart()
    {
        
    }

    protected override void OnDeath()
    {
        if (TryGetComponent(out Enemy enemy))
            Destroy(enemy);

        int coinCount = Random.Range(_onDeathMinCoinsCount, _onDeathMaxCoinsCount + 1);

        for (int i = 0; i < coinCount; i++)
            SpawnCoin();
    }

    private void SpawnCoin()
    {
        Vector3 position = new
        (
            x: transform.position.x + Random.Range(-_coinSpawnPositionHorizontalOffset, _coinSpawnPositionHorizontalOffset),
            y: transform.position.y,
            z: 0f
        );

        Instantiate(_coinPrefab, position, Quaternion.identity);
    }
}
