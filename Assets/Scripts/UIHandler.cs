using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIHandler : ListFunctionsExtension
{
    [SerializeField] private string tagItems;
    [SerializeField] private List<GameObjectSpritesAssing> inventoryObjectSpritesAssings;

    public void InventoryUIChange()
    {
        if (GameBrainScript.Styles.Count == 0)
        {
            SetBaseSprites();
            return;
        }
        SetStyleSprites();
    }

    private void SetBaseSprites()
    {
        for (int i = 0; i < PlayerBrain.Inventory.Items.Length; i++)
        {
            for (int j = 0; j < inventoryObjectSpritesAssings.Count; j++)
            {
                if (PlayerBrain.Inventory.Items[i].ItemGO != inventoryObjectSpritesAssings[j].Item)
                {
                    continue;
                }
                PlayerBrain.Inventory.Items[i].InventoryPlace.GetComponent<Image>().sprite = inventoryObjectSpritesAssings[j].BaseSprite;
            }
        }
    }

    private void SetStyleSprites()
    {
        for (int i = 0; i < PlayerBrain.Inventory.Items.Length; i++)
        {
            for (int j = 0; j < inventoryObjectSpritesAssings.Count; j++)
            {
                if (PlayerBrain.Inventory.Items[i].ItemGO != inventoryObjectSpritesAssings[j].Item)
                {
                    continue;
                }
                for (int k = 0; k < inventoryObjectSpritesAssings[j].StyleSprites.Length; k++)
                {
                    if (inventoryObjectSpritesAssings[j].StyleSprites[k].Style != GameBrainScript.PresentStyle)
                    {
                        continue;
                    }
                    PlayerBrain.Inventory.Items[i].InventoryPlace.GetComponent<Image>().sprite = inventoryObjectSpritesAssings[j].StyleSprites[k].Sprite;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        GameObject[] tmpItems = GameObject.FindGameObjectsWithTag(tagItems);

        ListCompare(inventoryObjectSpritesAssings, tmpItems.ToList(), () => new GameObjectSpritesAssing());
    }
}

[Serializable]
public class GameObjectSpritesAssing : ITranslate
{
    public GameObject Item;
    public Sprite BaseSprite;
    public StyleSpriteAssing[] StyleSprites = new StyleSpriteAssing[Enum.GetNames(typeof(GameBrainScript.Style)).Length];

    public GameObject GOTranslate
    {
        get { return Item; }
        set { Item = value; }
    }
}

[Serializable]
public class StyleSpriteAssing
{
    public Sprite Sprite;
    public GameBrainScript.Style Style;
}
