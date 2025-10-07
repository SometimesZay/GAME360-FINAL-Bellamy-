using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject collectablePrefab;
    public float spawnRate = 8f;
    public Transform[] spawnPoints;

    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCollectable();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void SpawnCollectable()
    {
        if (collectablePrefab && spawnPoints.Length > 0)
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(collectablePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }
}
