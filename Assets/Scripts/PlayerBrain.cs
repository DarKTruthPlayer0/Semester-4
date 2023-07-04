using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagItem;
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagInventoryPlaces;

    public static Inventory Inventory = new();

    private PathChoose pathChoose;
    private UIHandler uIHandler;
    private int selectedItemID;

    public void Interaction(GameObject IGO)
    {
        if (IGO.CompareTag(tagInteractable))
        {
            print("Interactable");
            if (Inventory.Items[selectedItemID].ItemSelected)
            {
                CheckInteractionOutcome(IGO);
            }
            else
            {
                InteractionWithoutItem(IGO);
            }
        }
        if (IGO.CompareTag(tagTalkableNPCs))
        {
            print("TalkToNPC");
        }
        if (IGO.CompareTag(tagItem))
        {
            print("ItemDetected");
            PickupItem(IGO);
        }
    }

    public void SelectItemInInventory(Button Button)
    {
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Button == Inventory.Items[i].InventoryPlace && !Inventory.Items[i].ItemSelected && Inventory.Items[i].ItemGO != null)
            {
                Inventory.Items[i].ItemSelected = true;
                selectedItemID = i;

                for (int j = 0; j < Inventory.Items.Length; j++)
                {
                    if (j == selectedItemID)
                    {
                        continue;
                    }
                    Inventory.Items[j].ItemSelected = false;
                }
                break;
            }
            else if (Button == Inventory.Items[i].InventoryPlace && Inventory.Items[i].ItemSelected && Inventory.Items[i].ItemGO != null)
            {
                Inventory.Items[i].ItemSelected = false;
                break;
            }
        }
    }

    private void InteractionWithoutItem(GameObject IWIGO)
    {
        for (int i = 0; i < DialogueOrganizer.DialogueClientsStatic.Length; i++)
        {
            if (DialogueOrganizer.DialogueClientsStatic[i].GOReference == IWIGO)
            {
                for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect.Count; j++)
                {
                    if (DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].UseThisDialogue && !DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].DialogueSpoken)
                    {
                        DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].DialogueSpoken = true;
                        DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].SelectedDialogue);
                        break;
                    }
                }
                break;
            }

        }
    }

    private void CheckInteractionOutcome(GameObject CIOGO)
    {
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic.Length; i++)
        {
            print("Test2");
            if (Inventory.Items[selectedItemID].ItemGO != ItemInteractionBrain.InteractionsStatic[i].Item || CIOGO != ItemInteractionBrain.InteractionsStatic[i].Interactable)
            {
                continue;
            }
            //Path Choose
            pathChoose.RoomToUnlockGO = ItemInteractionBrain.InteractionsStatic[i].RoomtoUnlock;
            pathChoose.InteractionID = i;
            pathChoose.SetButtons();
        }
    }
    private void PickupItem(GameObject TempItemGO)
    {
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Inventory.Items[i].ItemGO != null)
            {
                continue;
            }
            Inventory.Items[i].ItemGO = TempItemGO;
            //Dialog einblenden
            for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic.Length; j++)
            {
                if (TempItemGO != DialogueOrganizer.DialogueClientsStatic[j].GOReference)
                {
                    continue;
                }
                for (int k = 0; k < DialogueOrganizer.DialogueClientsStatic[j].DialogueSelect.Count; k++)
                {
                    if (!DialogueOrganizer.DialogueClientsStatic[j].DialogueSelect[k].UseThisDialogue)
                    {
                        continue;
                    }
                    DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[j].DialogueSelect[k].SelectedDialogue);
                    break;
                }
                break;
            }
            break;
        }
        TempItemGO.SetActive(false);
        uIHandler.InventoryUIChange();
    }

    private void TalkWithNPC(GameObject TWNPCGO)
    {
        for (int i = 0; i < DialogueOrganizer.DialogueClientsStatic.Length; i++)
        {
            if (TWNPCGO != DialogueOrganizer.DialogueClientsStatic[i].GOReference)
            {
                continue;
            }
            for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect.Count; j++)
            {
                if (DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].UseThisDialogue)
                {
                    DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect[j].SelectedDialogue);
                    break;
                }
            }
            break;
        }
    }

    private void SetUpIventory()
    {
        GameObject[] tempButtonGOs = GameObject.FindGameObjectsWithTag(tagInventoryPlaces);

        Inventory.Items = new Item[tempButtonGOs.Length];
        for (int j = 0;j < tempButtonGOs.Length; j++)
        {
            Inventory.Items[j] = new Item
            {
                InventoryPlace = tempButtonGOs[j].GetComponent<Button>()
            };
        }
    }

    private void Start()
    {
        GameObject[] tmpTTNGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tmpTWIIOGOs = GameObject.FindGameObjectsWithTag(tagInteractable);
        GameObject[] tmpTIGOs = GameObject.FindGameObjectsWithTag(tagItem);

        foreach (GameObject go in tmpTTNGOs)
        {
            go.AddComponent<PlayerServant>().pBrain = this;
        }
        foreach (GameObject go in tmpTWIIOGOs)
        { 
            go.AddComponent<PlayerServant>().pBrain = this;
        }
        foreach(GameObject go in tmpTIGOs)
        { 
            go.AddComponent<PlayerServant>().pBrain = this;
        }

        pathChoose = FindObjectOfType<PathChoose>();
        uIHandler = FindObjectOfType<UIHandler>();

        SetUpIventory();
    }
}

[Serializable]
public class Inventory
{
    public Item[] Items;
}
[Serializable]
public class Item
{
    public GameObject ItemGO;
    public Button InventoryPlace;
    public bool ItemSelected;
}
public class PlayerServant : MonoBehaviour
{
    [HideInInspector] public PlayerBrain pBrain;

    public void MouseDown()
    {
        pBrain.Interaction(gameObject);
    }
}