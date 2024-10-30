using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;

    private void Update()
    {
        if (playerHealth != null && healthSlider != null)
        {
            healthSlider.value = playerHealth.currentHealth;
        }
    }
}
