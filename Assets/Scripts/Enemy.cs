using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 1;
    float speed = 2f;

    [Header("Experience Points")]
    public int xpSpawnAmount = 1; // How many xp prefabs spawns from this enemy
    public GameObject xpPrefab;
    //[Header("AI")]
    //public float detectionRange = 5f;

    //private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find player
        //GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        //if (playerObj) player = playerObj.transform;
    }

    private void Update()
    {
        SetDownwardVelocity();
    }

    private void SetDownwardVelocity()
    {
        // Adjust speed based on GameManager timeElapsed

        rb.linearVelocity = Vector2.down * speed;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); // the enemy gets destroyed
        // Spawn XP pickups
        for (int i = 0; i < xpSpawnAmount; i++)
        {
            Instantiate(xpPrefab, transform.position, Quaternion.identity);
        }
        EventManager.TriggerEvent("OnEnemyKilled"); //update the score of the player
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    /*
    private void OnDrawGizmosSelected()
    {
        // Custom 2D circle for older Unity versions
        Gizmos.color = Color.red;

        int segments = 32;
        float angle = 0f;
        Vector3 lastPos = transform.position + new Vector3(detectionRange, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            angle = (i * 360f / segments) * Mathf.Deg2Rad;
            Vector3 newPos = transform.position + new Vector3(
                Mathf.Cos(angle) * detectionRange,
                Mathf.Sin(angle) * detectionRange,
                0
            );
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
    }
    */
}
