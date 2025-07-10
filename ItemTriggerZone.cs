using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerZone : MonoBehaviour
{
    public GameObject hiddenWall;
    public AudioClip pickupSound;
    private AudioSource audioSource;
    public GameObject npc;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Item'e temas edildi!");

            if (hiddenWall != null)
            {
                Debug.Log("Gizli duvar kaldırılıyor...");
                hiddenWall.SetActive(false);
            }

            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            if (npc != null)
            {
                DialogueTrigger trigger = npc.GetComponent<DialogueTrigger>();
                if (trigger != null)
                {
                    trigger.AdvanceStage();
                    Debug.Log("NPC'nin diyalog aşaması ilerletildi!");
                }
            }

            //? Item’ı anında yok etmeden önce sesi oynatabilmek için biraz bekleterek yok edebiliriz
            Destroy(gameObject, 0.1f);
        }
    }

}
