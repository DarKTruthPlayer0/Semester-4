using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    private Dialogues dialogues;
    private TMP_Text dialogueText;
    private int i;
    private static int dialogueID;

    public static void EnterDialogue(int DialogueID)
    {
        dialogueID = DialogueID;
        //activate Dialogue Object
    }

    public void NextPartOfDialogue()
    {
        if (dialogues.DialogueList[dialogueID].DialogueParts.Length < i)
        {
            i++;
            dialogueText.text = dialogues.DialogueList[dialogueID].DialogueParts[i].SentenceThePersonTalk;
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

[Serializable]
public class DialogueClients
{
    public GameObject GOReference;
    public int dialogueID;
}
