using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 1;
    public float baseSpeed = 2f;          // starting fall speed
    public float speed1000 = 3f;          // speed after score > 1000
    public float speed2000 = 4f;

    //[Header("AI")]
    //public float detectionRange = 5f;

    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;
    }

    private void Update()
    {
        SetDownwardVelocity();
    }

    private void SetDownwardVelocity()
    {
        float s = baseSpeed;
        if (GameManager.Instance.score > 2000) s = speed2000;
        else if (GameManager.Instance.score > 1000) s = speed1000;

        rb.linearVelocity = Vector2.down * s;
    }

    // Code Rewrite
    // Enemys will now path downwards
    // Goal: Arcade raining enemies feeling
    /*
    private void ChasePlayer()
    {
        if (player)
        {

            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
               rb.linearVelocity = direction * moveSpeed;
               rb.AddForce(direction * moveSpeed);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
            
        }
    }*/
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
        // This is where Singleton shines!
        // Any enemy can easily notify the GameManager
        GameManager.Instance.EnemyKilled(); //update the score of the player
        Destroy(gameObject); // the enemy gets destroyed
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
