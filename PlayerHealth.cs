using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private bool isDead = false;

    public delegate void OnHealthChanged(int current, int max);
    public static event OnHealthChanged HealthChanged;

    public delegate void OnPlayerDied();
    public static event OnPlayerDied PlayerDied;

    void Start()
    {
        currentHealth = maxHealth;
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        HealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        PlayerDied?.Invoke();
    }

    public bool IsDead()
    {
        return isDead;
    }
}