using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;
    public float spawnRate = 1.7f;
    public Transform[] spawnPoints;

    private float nextSpawnTime = 0f;
    private bool isGameOver = false;

    private void Awake()
    {
        EventManager.Subscribe("OnPlayerDied", OnGameOver);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDied", OnGameOver);
    }

    private void Update()
    {
        if (isGameOver) return; // Stop spawning if game is over

        // Adjust spawn rate based on GameManager timeElapsed
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    private void OnGameOver(object _ = null)
    {
        isGameOver = true;
    }
}
