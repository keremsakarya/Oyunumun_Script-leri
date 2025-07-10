using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    public int damageAmount = 1;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHealth == null || playerHealth.IsDead()) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Trap"))
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
