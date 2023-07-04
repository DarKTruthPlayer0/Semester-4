using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathChoose : MonoBehaviour
{
    public GameObject PathChooseGO;
    public GameObject RoomToUnlockGO;
    public int InteractionID;

    [SerializeField] private Button[] decisionButtons;
    private GameObject tempPathChooseButtonsGO;
    private bool helperBool;

    public void SetButtons()
    {
        tempPathChooseButtonsGO.SetActive(true);
        List<GameBrainScript.Style> tmpStyles = new();
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic[InteractionID].Paths.Length; i++)
        {
            tmpStyles.Add(ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].Style);
        }
        string[] tempStyleNames = Enum.GetNames(typeof(GameBrainScript.Style));

        for (int i = 0; i < tempPathChooseButtonsGO.transform.childCount; i++)
        {
            tempPathChooseButtonsGO.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = tempStyleNames[i];
            for (int j = 0; j < tmpStyles.Count; j++)
            {
                if (tmpStyles[j].ToString() == tempStyleNames[i])
                {
                    helperBool = true;
                    break;
                }
                else
                {
                    helperBool = false;
                }
            }
            if (helperBool)
            {
                continue;
            }
            tempPathChooseButtonsGO.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DecisionTaken(GameObject GO)
    {
        string[] tempStylenames = Enum.GetNames(typeof(GameBrainScript.Style));
        for (int i = 0; i < tempStylenames.Length; i++)
        {
            if (GO.transform.GetChild(0).GetComponent<TMP_Text>().text == Enum.GetName(typeof(GameBrainScript.Style), i))
            {
                GameBrainScript.Styles.Add((GameBrainScript.Style)i);
                GameBrainScript.CalculatePresentStyle();
                StartInteractionEvent((GameBrainScript.Style)i);
                break;
            }
        }
        tempPathChooseButtonsGO.SetActive(false);
    }

    private void StartInteractionEvent(GameBrainScript.Style style)
    {
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic[InteractionID].Paths.Length; i++)
        {
            if (ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].Style == style)
            {
                for (int j = 0; j < ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].DialogueSelect.Count; j++)
                {
                    //Start Dialogue
                    if (ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].DialogueSelect[j].UseThisDialogue)
                    {
                        for (int k = 0; k < RoomOrganizer.RoomsStatic.Length; k++)
                        {
                            if (RoomOrganizer.RoomsStatic[k].RoomGO == RoomToUnlockGO && RoomToUnlockGO != null)
                            {
                                RoomOrganizer.RoomsStatic[k].IsLocked = false;
                                RoomToUnlockGO = null;
                                break;
                            }

                        }
                        DialogueSystem.EnterDialogue(ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].DialogueSelect[j].SelectedDialogue);
                        break;
                    }
                }
                break;
            }
        }
    }

    private void Awake()
    {
        tempPathChooseButtonsGO = GameObject.Find("PathChooseButtons");
        tempPathChooseButtonsGO.SetActive(false);
    }
}