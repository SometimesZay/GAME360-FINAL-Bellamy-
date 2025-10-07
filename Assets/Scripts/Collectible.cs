using System;
using UnityEngine;
public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int value = 50;
    public float rotationSpeed = 90f;
    public float speed = 2f;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetDownwardVelocity();
    }
    private void SetDownwardVelocity()
    {
        if (GameManager.Instance.score > 4000) speed = 6f;
        else if (GameManager.Instance.score > 3000) speed = 5f;
        else if (GameManager.Instance.score > 2000) speed = 4f;
        else if (GameManager.Instance.score > 1000) speed = 3f;

        rb.linearVelocity = Vector2.down * speed;
    }


    private void Update()
    {
        // Rotate for visual effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Easy access to GameManager through Singleton!
            GameManager.Instance.CollectiblePickedUp(value);
            Destroy(gameObject);
        }
        if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}