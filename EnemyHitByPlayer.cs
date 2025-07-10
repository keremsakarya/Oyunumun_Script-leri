using System.Collections;
using UnityEngine;

public class EnemyHitByPlayer : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip deathSfx;

    public int maxHealth = 3;
    private int currentHealth;

    private bool recentlyHit = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox") && !recentlyHit)
        {
            Debug.Log("Düşman hasar aldı!");

            currentHealth--;

            if (currentHealth <= 0)
            {
                if (explosionPrefab != null)
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                if (deathSfx != null)
                    AudioSource.PlayClipAtPoint(deathSfx, transform.position);

                Destroy(gameObject);
            }
            else
            {
                // Yeniden vurulmayı kısa süreliğine engelle
                StartCoroutine(HitCooldown());
            }
        }
    }

    IEnumerator HitCooldown()
    {
        recentlyHit = true;
        yield return new WaitForSeconds(0.3f); // Aynı saldırı animasyonunda 1 kez vurulabilsin
        recentlyHit = false;
    }
}
