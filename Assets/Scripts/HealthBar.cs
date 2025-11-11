using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private float maxHealth = 100f;

    void Start()
    {
        // Initialize the slider
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        // Subscribe to health change events
        EventManager.Subscribe("OnHealthChanged", OnHealthChanged);
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe("OnHealthChanged", OnHealthChanged);
    }

    void OnHealthChanged(object value)
    {
        float currentHealth = (float)value;

        if (slider != null)
        {
            slider.value = currentHealth;
        }
    }

    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = Mathf.Clamp(health, 0, maxHealth);
        }
    }
}


