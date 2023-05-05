using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    private Dialogues dialogues;
    private TMP_Text dialogueText;
    private int i;
    private int dialogueID;

    public void EnterDialogue(int DialogueID)
    {
        dialogueID = DialogueID;
        //activate Dialogue Object
    }

    public void NextSentenceOfDialogue()
    {
        if (dialogues.DialogueList[dialogueID].Sentences.Count < i)
        {
            i++;
            dialogueText.text = dialogues.DialogueList[dialogueID].Sentences[i];
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        dialogueText.text = null;
        //deactivate Dialogue Object
    }

    private void Start()
    {
        dialogues = FindObjectOfType<Dialogues>();
    }
}

public class DialogueClient : MonoBehaviour
{
    public int dialogueID;
}
