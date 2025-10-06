using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip CoinSound;
    private AudioSource audioSource;//Unity componenet

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f; // Adjust volume as needed

    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, 0).normalized; // Disable vertical movement for side-scrolling
        rb.linearVelocity = movement * moveSpeed;
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }

    }

    private void FireBullet()
    {
        if (GameManager.Instance.score > 5000) fireRate = .1f;
        else if (GameManager.Instance.score > 2000) fireRate = .2f;
        else if (GameManager.Instance.score > 1000) fireRate = .3f;
        else if (GameManager.Instance.score > 500) fireRate = .5f;
        else if (GameManager.Instance.score > 250) fireRate = .8f;

        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        audioSource.PlayOneShot(ShootSound);
        // Play shoot sound effect
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Player hit by enemy - lose a life
            GameManager.Instance.LoseLife();
        }

        if (other.CompareTag("Collectible"))
        {
            // Player collected an item
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible)
            {
                GameManager.Instance.CollectiblePickedUp(100);
                audioSource.PlayOneShot(CoinSound);
                Destroy(other.gameObject);


            }
        }
    }
}
