using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [HideInInspector] public bool StartSpawn = false;
    [SerializeField] private Collider2D SpawnArea;
    [SerializeField] private Range SpawnInterval;
    [SerializeField] private List<PowerUp> PowerUps = new List<PowerUp>();
    [SerializeField] public GameObject PowerUpPanelP1;
    [SerializeField] public GameObject PowerUpPanelP2;

    [SerializeField] private int maxPowerUpInScene = 3;

    public static PowerUpSpawner Instance { get; private set; }
    public static int powerUpInScene = 0;

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

    private void SpawnPowerUp()
    {
        if (powerUpInScene >= maxPowerUpInScene) return;
        int chosen = Random.Range(0, PowerUps.Count - 1);
        PowerUp spawned = Instantiate(PowerUps[chosen], GetRandomPoint(), new Quaternion());
        spawned.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    private IEnumerator WaitInterval()
    {
        StartSpawn = false;
        yield return new WaitForSeconds(SpawnInterval.RandomValue());
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
            SpawnPowerUp();
            StartCoroutine(WaitInterval());
        }
    }
}
