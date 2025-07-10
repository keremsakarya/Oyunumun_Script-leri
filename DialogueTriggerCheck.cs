using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerCheck : MonoBehaviour
{
    private DialogueSystem dialogueSystem;

    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            DialogueTrigger trigger = other.GetComponent<DialogueTrigger>();
            if (trigger != null && trigger.CanTalk())
            {
                string[] lines = trigger.GetLines();

                bool shouldTransition = false;

                //? Stage == 1 ise ikinci diyalogdayız, geçiş yapılacak
                if (trigger.CurrentStage() == 1)
                {
                    shouldTransition = true;
                }

                dialogueSystem.StartDialogue(lines, shouldTransition);
            }
        }
    }
}
