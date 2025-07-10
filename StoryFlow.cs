using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryFlow : MonoBehaviour
{
    public GameObject storyPanel;
    public TMP_Text storyText;
    public string[] storyLines;
    private int currentLine = 0;
    private bool isShowing = false;
    public MonoBehaviour playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        if (StoryState.storyShown)
        {
            storyPanel.SetActive(false);
            if (playerMovement != null)
                playerMovement.enabled = true;
            return;
        }

        storyPanel.SetActive(false);
        if (playerMovement != null)
            playerMovement.enabled = false;

        StartCoroutine(ShowStory());
    }

    IEnumerator ShowStory()
    {
        yield return new WaitForSeconds(1f);
        storyPanel.SetActive(true);
        storyText.text = storyLines[currentLine];
        isShowing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.E))
        {
            currentLine++;

            if (currentLine < storyLines.Length)
            {
                storyText.text = storyLines[currentLine];
            }
            else
            {
                storyPanel.SetActive(false);
                isShowing = false;
                currentLine = 0;

                if (playerMovement != null)
                    playerMovement.enabled = true;

                StoryState.storyShown = true;
            }
        }
    }
}
