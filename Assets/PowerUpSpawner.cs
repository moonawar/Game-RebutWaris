using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    public bool StartSpawn = false;
    [SerializeField] private Collider2D SpawnArea;
    [SerializeField] private float SpawnInterval;
    [SerializeField] private List<PowerUp> PowerUps = new List<PowerUp>();

    public static PowerUpSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private IEnumerator SpawnPowerUp()
    {
        int chosen = Random.Range(0, PowerUps.Count - 1);
        PowerUp spawned = Instantiate(PowerUps[chosen], GetRandomPoint(), new Quaternion());
        spawned.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        yield return null;

    }

    private IEnumerator WaitInterval()
    {
        StartSpawn = false;
        yield return new WaitForSeconds(SpawnInterval);
        StartSpawn = true;
    }

    private Vector3 GetRandomPoint()
    {
        Vector2 randomPoint = new Vector3(
            Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x),
            Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y), 0
        );

        return randomPoint;
    }

    private void FixedUpdate()
    {
        if (StartSpawn)
        {
            StartCoroutine(WaitInterval());
            StartCoroutine(SpawnPowerUp());
        }
    }
}
