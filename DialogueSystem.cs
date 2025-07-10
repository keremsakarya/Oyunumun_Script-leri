using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public MonoBehaviour playerMovement;
    public Image fadePanel;

    private string[] lines;
    private int currentLine = 0;
    private bool isTalking = false;
    private bool endAndTransition = false; //? Sahne geçişi
    public float fadeDuration = 6f;

    // Update is called once per frame
    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.E))
        {
            currentLine++;

            if (currentLine < lines.Length)
            {
                dialogueText.text = lines[currentLine];
            }
            else
            {
                if (endAndTransition)
                {
                    StartCoroutine(EndAndTransitionRoutine());
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void StartDialogue(string[] newLines, bool shouldTransition = false)
    {
        lines = newLines;
        currentLine = 0;
        isTalking = true;
        endAndTransition = shouldTransition;

        dialoguePanel.SetActive(true);
        dialogueText.text = lines[currentLine];
        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;
        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    IEnumerator EndAndTransitionRoutine()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;

        //? Sesleri durdur
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudio)
        {
            audio.Stop();
        }

        //? Fadeout kısmı
        if (fadePanel != null)
        {
            float timer = 0f;
            Color color = fadePanel.color;

            while (timer < fadeDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadePanel.color = new Color(color.r, color.g, color.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }

            //* Bir kez daha siyah yap
            fadePanel.color = new Color(color.r, color.g, color.b, 1f);
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SecondScene");
    }
}
