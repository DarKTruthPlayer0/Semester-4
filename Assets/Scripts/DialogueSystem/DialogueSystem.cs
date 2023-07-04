using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private static TMP_Text dialogueName;
    private static TMP_Text dialogueText;
    private static int i = 0;
    private static Dialogue tmpDialogue;
    private static GameObject dialogueWindowGO;

    private static Image portraitImage;
    private static AudioSource audioSourceEmotions;

    public static void EnterDialogue(Dialogue dialogue)
    {
        tmpDialogue = dialogue;
        //activate Dialogue Object
        dialogueName.text = tmpDialogue.DialogueParts[i].PersonNameWhichTalks;
        dialogueText.text = tmpDialogue.DialogueParts[i].SentenceThePersonTalk;

        SetSpriteAndSound();

        i = 1;
        dialogueWindowGO.SetActive(true);
    }

    public void NextPartOfDialogue()
    {
        print("next DialoguePart");
        if (i < tmpDialogue.DialogueParts.Length)
        {
            dialogueName.text = tmpDialogue.DialogueParts[i].PersonNameWhichTalks;
            dialogueText.text = tmpDialogue.DialogueParts[i].SentenceThePersonTalk;

            SetSpriteAndSound();

            i++;
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        i = 0;
        tmpDialogue = null;
        dialogueName.text = null;
        dialogueText.text = null;
        //deactivate Dialogue Object
        dialogueWindowGO.SetActive(false);
    }

    private static void SetSpriteAndSound()
    {
        for (int i = 0; i < tmpDialogue.DialogueParts[i].Emotions.Length; i++)
        {
            if (tmpDialogue.DialogueParts[i].Emotions[i].style == GameBrainScript.PresentStyle)
            {
                if (tmpDialogue.DialogueParts[i].Emotions[i].EmotionSprite != null)
                {
                    portraitImage.sprite = tmpDialogue.DialogueParts[i].Emotions[i].EmotionSprite;
                }

                if (tmpDialogue.DialogueParts[i].Emotions[i].EmotionSound != null)
                {
                    audioSourceEmotions.clip = tmpDialogue.DialogueParts[i].Emotions[i].EmotionSound;
                    audioSourceEmotions.Play();
                }
            }
        }
    }
    private void Start()
    {
        dialogueWindowGO = GameObject.Find("DialogueWindow");
        dialogueName = dialogueWindowGO.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        dialogueText = dialogueWindowGO.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();

        portraitImage = GameObject.Find("PortraitImage").GetComponent<Image>();
        audioSourceEmotions = GameObject.Find("EmotionSounds").GetComponent<AudioSource>();
        dialogueWindowGO.SetActive(false);
    }
}
