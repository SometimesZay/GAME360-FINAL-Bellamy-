using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        EventManager.TriggerEvent("OnHealthChanged", currentHealth);
    }

    public void TakeDamage(float amount)
    {
        EventManager.TriggerEvent("OnPlayerHurt");
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("HP: " + currentHealth);
        EventManager.TriggerEvent("OnHealthChanged", currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        EventManager.TriggerEvent("OnPlayerDied");
        GameManager.Instance?.PauseGame();
        GetComponent<PlayerController>().enabled = false;
    }
}






