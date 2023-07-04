using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class UIHandler : ListFunctionsExtension
{
    [SerializeField] private string tagItems;
    [SerializeField] private List<GameObjectSpritesAssing> inventoryObjectSpritesAssings;

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
    public StyleSpriteAssing[] StyleSpites = new StyleSpriteAssing[Enum.GetNames(typeof(GameBrainScript.Style)).Length];

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
