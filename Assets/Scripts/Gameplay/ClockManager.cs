using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public static ClockManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    [SerializeField] private BoxCollider2D _arena;
    [SerializeField] private GameObject _clockPrefab;

    public void SpawnClock()
    {
        Vector2 spawnPoint = GetRandomSpawnPoint();
        Instantiate(_clockPrefab, spawnPoint, Quaternion.identity);
    }

    private Vector2 GetRandomSpawnPoint()
    {
        float x = Random.Range(_arena.bounds.min.x, _arena.bounds.max.x);
        float y = Random.Range(_arena.bounds.min.y, _arena.bounds.max.y);
        return new Vector2(x, y);
    }
}
