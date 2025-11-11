using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject collectablePrefab;
    public float spawnRate = 8f;
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
            SpawnCollectable();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnCollectable()
    {
        if (collectablePrefab == null || spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(collectablePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
    private void OnGameOver(object _ = null)
    {
        isGameOver = true;
    }
}
