using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private static TMP_Text dialogueName;
    private static TMP_Text dialogueText;
    private static int dialoguePartIndicator = 0;
    private static Dialogue tmpDialogue;
    private static GameObject dialogueWindowGO;

    private static Image portraitImage;
    private static AudioSource audioSourceEmotions;

    public static void EnterDialogue(Dialogue dialogue)
    {
        tmpDialogue = dialogue;
        print(dialogue.DialogueParts[dialoguePartIndicator].Emotions.Length);
        //activate Dialogue Object
        dialogueName.text = tmpDialogue.DialogueParts[dialoguePartIndicator].PersonNameWhichTalks;
        dialogueText.text = tmpDialogue.DialogueParts[dialoguePartIndicator].SentenceThePersonTalk;

        SetSpriteAndSound();

        dialoguePartIndicator = 1;
        dialogueWindowGO.SetActive(true);
    }

    public void NextPartOfDialogue()
    {
        print("next DialoguePart");
        if (dialoguePartIndicator < tmpDialogue.DialogueParts.Length)
        {
            dialogueName.text = tmpDialogue.DialogueParts[dialoguePartIndicator].PersonNameWhichTalks;
            dialogueText.text = tmpDialogue.DialogueParts[dialoguePartIndicator].SentenceThePersonTalk;

            SetSpriteAndSound();

            dialoguePartIndicator++;
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        dialoguePartIndicator = 0;
        tmpDialogue = null;
        dialogueName.text = null;
        dialogueText.text = null;
        //deactivate Dialogue Object
        dialogueWindowGO.SetActive(false);
    }

    private static void SetSpriteAndSound()
    {
        
        for (int i = 0; i < tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions.Length; i++)
        {
            if (tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions[i].Style == GameBrainScript.PresentStyle)
            {
                if (tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions[i].EmotionSprite != null)
                {
                    portraitImage.sprite = tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions[i].EmotionSprite;
                }

                if (tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions[i].EmotionSound != null)
                {
                    audioSourceEmotions.clip = tmpDialogue.DialogueParts[dialoguePartIndicator].Emotions[i].EmotionSound;
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
