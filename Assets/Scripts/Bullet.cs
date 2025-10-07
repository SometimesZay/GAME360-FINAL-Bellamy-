using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Flight")]
    public float speed = 12f;
    public float lifetime = 2f;

    [Header("Hit")]
    public float hitDuration = .583f;

    Rigidbody2D rb;
    Collider2D col;
    Animator anim;
    bool hit;

    void Awake()
{
    rb   = GetComponent<Rigidbody2D>();
    col  = GetComponent<Collider2D>();
    anim = GetComponentInChildren<Animator>();

    rb.bodyType = RigidbodyType2D.Kinematic; 
    rb.gravityScale = 0f; 
}

    void OnEnable()
    {
        // flight start
        rb.linearVelocity = transform.up * speed;
        Invoke(nameof(SelfDestruct), lifetime);    // cancelable lifetime
    }

    void SelfDestruct() => Destroy(gameObject);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(1);
            }
            Impact();
        }
        else if (other.CompareTag("Boundary")) 
        {
            Impact();
        }
    }

    void Impact()
    {
        hit = true;

        // stop moving & stop colliding
        rb.linearVelocity = Vector2.zero;
        if (col) col.enabled = false;

        // play hit animation
        if (anim) anim.SetTrigger("HITENEMY");

        // ensure lifetime doesn’t cut the animation
        CancelInvoke(nameof(SelfDestruct));
        Destroy(gameObject, hitDuration);
    }
}