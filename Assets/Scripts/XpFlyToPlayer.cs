using UnityEngine;

public class XpFlyToPlayer : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private bool attracted = false;

    [Header("Attraction Settings")]
    public float delayBeforeAttract = 0.5f;
    public float attractionSpeed = 5f;
    public float pickupDistance = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(StartAttract), delayBeforeAttract);
    }

    void StartAttract()
    {
        attracted = true;
        if (rb != null) rb.gravityScale = 0; // optional: stop gravity effect
    }

    void Update()
    {
        if (!attracted || target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        rb.linearVelocity = direction * attractionSpeed;

        // When close enough, simulate collection
        if (distance < pickupDistance)
        {
            Debug.Log("Pickup collected!");
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }
}

