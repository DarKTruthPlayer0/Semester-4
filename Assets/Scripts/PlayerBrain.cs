using System;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagItem;
    [SerializeField] private string tagWithItemInteractableObject;
    [SerializeField] private string tagTalkableNPCs;

    private Inventory inventory = new();
    private int selectedItemID;

    public void Interaction(GameObject GO)
    {
        if (GO.CompareTag(tagWithItemInteractableObject) /*&& inventory.Item != null*/)
        {
            print("InteractWithObject");
        }
        if (GO.CompareTag(tagTalkableNPCs))
        {
            print("TalkToNPC");
        }
        if (GO.CompareTag(tagItem))
        {
            print("ItemDetected");
            PickupItem(GO);
        }
    }

    private void UseItemInIventory(GameObject UIIIGO)
    {
        for (int i = 0; i < ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic.Length; i++)
        {
            if (ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.Item != inventory.Items[selectedItemID].ItemGO)
            {
                continue;
            }
            for (int j = 0; j < ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.InteractionObjects[j].InteractWithObject)
                {

                    break;
                    //Dialog einblenden
                }
            }
        }
    }

    private void PickupItem(GameObject ItemGO)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i] == null)
            {
                ItemGO.SetActive(false);
                inventory.Items[i].ItemGO = ItemGO;
                //Dialog einblenden
                break;
            }
        }
    }
    private void Start()
    {
        GameObject[] tmpTTNGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tmpTWIIOGOs = GameObject.FindGameObjectsWithTag(tagWithItemInteractableObject);
        GameObject[] tmpTIGOs = GameObject.FindGameObjectsWithTag(tagItem);

        foreach (GameObject go in tmpTTNGOs)
        {
            go.AddComponent<PlayerServant>();
        }
        foreach (GameObject go in tmpTWIIOGOs)
        { 
            go.AddComponent<PlayerServant>();
        }
        foreach(GameObject go in tmpTIGOs)
        { 
            go.AddComponent<PlayerServant>();
        }
    }
}

[Serializable]
public class Inventory
{
    public Item[] Items = new Item[3];
}
[Serializable]
public class Item
{
    public GameObject ItemGO;
    public bool ItemSelected;
}
public class PlayerServant : MonoBehaviour
{
    private PlayerBrain pBrain;

    private void OnMouseDown()
    {
        pBrain.Interaction(gameObject);
    }

    private void Start()
    {
        pBrain = FindObjectOfType<PlayerBrain>();
    }
}