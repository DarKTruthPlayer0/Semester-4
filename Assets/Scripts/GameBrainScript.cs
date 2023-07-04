using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBrainScript : MonoBehaviour
{
    public static List<Style> Styles = new List<Style>();
    public enum Style
    {
        Horror,
        Datingsim,
        Cyberpunk
    }
    public static Style PresentStyle = Style.Datingsim;

    [SerializeField] private StyleClass[] styleClasses;
    private static StyleClass[] styleClassesStatic;
    private static Camera cam;

    private static void SetStyle()
    {
        for (int i = 0; i < styleClassesStatic.Length; i++)
        {
            if (PresentStyle == styleClassesStatic[i].style)
            {
                cam.cullingMask = styleClassesStatic[i].layerMask;
            }
        }
    }

    public static void CalculatePresentStyle()
    {
        if (Styles.Count == 0)
        {
            return;
        }
        string[] enumArray = Enum.GetNames(typeof(Style));
        int[] tmpStyleVoteNumbers = new int[enumArray.Length];

        for (int i = 0; i < enumArray.Length; i++)
        {
            for (int j = 0; j < Styles.Count; j++)
            {
                if (Styles[j] != (Style)i)
                {
                    continue;
                }
                tmpStyleVoteNumbers[i]++;
            }
        }

        int x = 0;
        int y = 0;
        for (int i = 0; i < tmpStyleVoteNumbers.Length; i++)
        {
            if (tmpStyleVoteNumbers[i] <= x)
            {
                continue;
            }
            x = tmpStyleVoteNumbers[i];
            y = i;
        }
        PresentStyle = (Style)y;
        print(PresentStyle);
        SetStyle();
    }

    private void Start()
    {
        cam = Camera.main;
        styleClassesStatic = styleClasses;
    }
}

[Serializable]
public class StyleClass
{
    public GameBrainScript.Style style;
    public LayerMask layerMask;
}