using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class DialogueOrganizer : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagItems;
    [SerializeField] private string tagInteractables;


    public static DialogueClients[] DialogueClientsStatic;
    [SerializeField] Dialogues dialogues;
    [SerializeField] private List<DialogueClients> dialogueClientsList;
    private bool assingmentBool;


    private void Start()
    {
        DialogueClientsStatic = dialogueClientsList.ToArray();
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            AssingDialogueClients();
            SetDialogueIDs();
        }
    }

    private void AssingDialogueClients()
    {
        GameObject[] tempTalkableNPCGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tempItemGOs = GameObject.FindGameObjectsWithTag(tagItems);
        GameObject[] tempInteractableGOs = GameObject.FindGameObjectsWithTag(tagInteractables);
        List<GameObject> tempDialogueClientGOs = new();
        for (int i = 0; i < tempTalkableNPCGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempTalkableNPCGOs[i]);
        }
        for (int i = 0; i < tempItemGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempItemGOs[i]);
        }
        for (int i = 0; i < tempInteractableGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempInteractableGOs[i]);
        }

        for (int i = 0; i < dialogueClientsList.Count; i++)
        {
            for (int j = 0; j < tempDialogueClientGOs.Count; j++)
            {
                if (dialogueClientsList[i].GOReference == tempDialogueClientGOs[j])
                {
                    assingmentBool = true;
                    break;
                }
                else
                {
                    assingmentBool = false;
                }
            }
            if (assingmentBool)
            {
                continue;
            }
            dialogueClientsList.RemoveAt(i);
        }

        for (int i = 0; i < tempDialogueClientGOs.Count; i++)
        {
            for (int j = 0; j < dialogueClientsList.Count; j++)
            {
                if (dialogueClientsList[j].GOReference == tempDialogueClientGOs[i])
                {
                    assingmentBool = true;
                    break;
                }
                else
                {
                    assingmentBool = false;
                }
            }
            if (assingmentBool)
            {
                continue;
            }
            DialogueClients tmpDialogueClient = new()
            {
                GOReference = tempDialogueClientGOs[i]
            };
            dialogueClientsList.Add(tmpDialogueClient);
        }
    }

    private void SetDialogueIDs()
    {
        for (int i = 0; i < dialogues.DialogueList.Count; i++)
        {
            dialogues.DialogueList[i].DialogueID = i;
        }
    }
}
