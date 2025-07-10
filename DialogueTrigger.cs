using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] firstDialogue;
    public string[] secondDialogue;

    private int stage = 0;
    private bool hasTalkedOnce = false;

    public bool CanTalk()
    {
        if (stage == 0)
            return !hasTalkedOnce;

        return true;
    }

    public string[] GetLines()
    {
        if (stage == 0)
        {
            hasTalkedOnce = true;
            return firstDialogue;
        }
        else
        {
            return secondDialogue;
        }
    }

    public int CurrentStage()
    {
        return stage;
    }

    public void AdvanceStage()
    {
        stage++;
    }
}
