using System.Collections;
using UnityEngine;
using TMPro;

public class SpeedPickup : MonoBehaviour
{
    public float speedMultiplier = 1.8f;
    public float duration = 7f;
    public string playerTag = "Player";

    public TMP_Text speedText;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplySpeedBoost(speedMultiplier, duration);
            }

            if (speedText != null)
            {
                speedText.text = "Speed increased for 7 seconds";
                speedText.gameObject.SetActive(true);
            }

            // Objenin görselini ve çarpışmasını kapat
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (collider2d != null) collider2d.enabled = false;

            // Coroutine çalışsın, 3 saniye sonra yazıyı da kapat ve objeyi yok et
            StartCoroutine(HideTextAndDestroySelf());
        }
    }

    private IEnumerator HideTextAndDestroySelf()
    {
        yield return new WaitForSeconds(3f);

        if (speedText != null)
        {
            speedText.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
