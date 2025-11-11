using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chest : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Transform spawnPosition; // <-- new reference

    [Header("Pickup Settings")]
    public GameObject pickupPrefab;
    public int pickupCount = 3;
    public float launchForce = 2f;

    private bool isOpened = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spawnPosition == null)
            spawnPosition = transform; // fallback to chest position if not assigned
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            isOpened = true;
            animator.SetTrigger("openChest");

            // Start coroutine for delayed spawn
            StartCoroutine(SpawnAfterDelay(other.transform, 0.75f)); // 0.75 seconds delay
        }
    }

    IEnumerator SpawnAfterDelay(Transform player, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPickups(player);
    }

    void SpawnPickups(Transform player)
    {
        for (int i = 0; i < pickupCount; i++)
        {
            Vector2 spawnPos = spawnPosition.position + (Vector3)Random.insideUnitCircle * 0.1f;
            GameObject pickup = Instantiate(pickupPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = pickup.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * launchForce, ForceMode2D.Impulse);
            }

            PickupFlyToPlayer flyScript = pickup.GetComponent<PickupFlyToPlayer>();
            if (flyScript != null)
                flyScript.SetTarget(player);
        }
    }
}
