using UnityEngine;

[ExecuteInEditMode]
public class DialogueOrganizer : MonoBehaviour
{
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] Dialogues dialogues;

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
        for (int i = 0; i < tempTalkableNPCGOs.Length; i++)
        {
            GameObject GO = tempTalkableNPCGOs[i];
            if (GO.GetComponent<DialogueClient>() == null)
            {
                print("Test");
                GO.AddComponent<DialogueClient>();
            }
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
