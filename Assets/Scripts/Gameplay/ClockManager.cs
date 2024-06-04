using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private float _fallRadius;
    [SerializeField] private float _offset = 2;

    public void SpawnClock()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Instantiate(_clockPrefab, spawnPoint, Quaternion.identity);
    }
    public void SpawnClock(Vector3 position)
    {
        Vector3 spawnPoint = GetRandomFallPoint(position);
        Instantiate(_clockPrefab, spawnPoint, Quaternion.identity);
    }


    private Vector3 GetRandomSpawnPoint()
    {
        float x = Random.Range(_arena.bounds.min.x, _arena.bounds.max.x);
        float y = Random.Range(_arena.bounds.min.y, _arena.bounds.max.y);
        return new Vector3(x, y, -1);
    }

    private Vector3 GetRandomFallPoint(Vector3 center)
    {
        float randomx = Random.Range(_fallRadius * -1, _fallRadius);
        float randomy = Random.Range(_fallRadius * -1, _fallRadius);

        if (Mathf.Abs(randomx) <= _offset)
        {
            randomx = randomx <= 0 ? randomx - _offset : randomx + -_offset;
        }

        if (Mathf.Abs(randomy) <= _offset)
        {
            randomy = randomy <= 0 ? randomy - _offset : randomy + _offset;
        }

        randomx = center.x + randomx < _arena.bounds.min.x ? _arena.bounds.min.x - center.x : randomx;
        randomx = center.x + randomx > _arena.bounds.max.x ? _arena.bounds.max.x - center.x : randomx;

        randomy = center.y + randomy < _arena.bounds.min.y ? _arena.bounds.min.y - center.y : randomy;
        randomy = center.y + randomy > _arena.bounds.max.y ? _arena.bounds.max.y - center.y : randomy;

        float x = center.x + randomx;
        float y = center.y + randomy;

        if (Mathf.Abs(x - center.x) <= _offset)
        {
            x = center.x - randomx;
        }
        if (Mathf.Abs(y - center.y) <= _offset)
        {
            y = center.y - randomy;
        }

        return new Vector3(x, y, -1);
    }
}
