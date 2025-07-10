using System.Collections;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (playerMovement != null && playerMovement.isAttacking)
            {
                Animator enemyAnimator = collision.collider.GetComponent<Animator>();
                if (enemyAnimator != null)
                {
                    enemyAnimator.SetTrigger("Die");
                }

                // Enemy objesini 0.5 saniye sonra yok et
                Destroy(collision.collider.gameObject, 0.5f);
            }
            else
            {
                Debug.Log("Saldırıda değil, hasar alınacak...");

                if (playerHealth != null && !playerHealth.IsDead())
                {
                    playerHealth.TakeDamage(1); // sadece can azaltılır, sahne yeniden yüklenmez
                }
            }
        }
    }
}
