using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider;

    void OnEnable()
    {
        PlayerHealth.HealthChanged += UpdateHealthBar;
    }

    void OnDisable()
    {
        PlayerHealth.HealthChanged -= UpdateHealthBar;
    }

    void UpdateHealthBar(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }
}
