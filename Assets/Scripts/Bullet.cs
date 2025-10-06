using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 2f;
    public int enemiesValue;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * speed;   // world space up


        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Bullet hit enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(1);
                GameManager.Instance.AddScore(enemiesValue);
                Destroy(gameObject); // Destroy bullet
            }
        }

        // Destroy bullet if it hits walls or boundaries
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall Hit");
        }
    }
}