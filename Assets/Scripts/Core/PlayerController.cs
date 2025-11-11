using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip CoinSound;
    private AudioSource audioSource;//Unity componenet

    [Header("Components")]
    internal PlayerState currentState;
    internal Animator animator;
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
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime || Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }

    }

    private void FireBullet()
    {
        if (GameManager.Instance.GetScore() > 10000) fireRate = .1f;
        else if (GameManager.Instance.GetScore() > 3000) fireRate = .2f;
        else if (GameManager.Instance.GetScore() > 2000) fireRate = .5f;
        else if (GameManager.Instance.GetScore() > 1000) fireRate = .8f;
        else if (GameManager.Instance.GetScore() > 500) fireRate = 1f;

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
            // Player hit by enemy - take damage
            PlayerHealth health = GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(20f); // Adjust damage value as desired
            }

            // destroy enemy after collision
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Collectible"))
        {
            // Player collected an item
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible)
            {
                GameManager.Instance.AddScore(100);
                audioSource.PlayOneShot(CoinSound);
                Destroy(other.gameObject);
                EventManager.TriggerEvent("OnPickupCollected");

            }
        }
    }
    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnPlayerStateChanged", currentState.GetStateName());
    }
}
