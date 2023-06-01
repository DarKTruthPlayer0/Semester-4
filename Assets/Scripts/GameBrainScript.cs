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

    [SerializeField] private StyleClass[] styleClasses;
    private Camera cam;
    private Style presentStyle;

    private void SetStyle()
    {
        for (int i = 0; i < styleClasses.Length; i++)
        {
            if (presentStyle == styleClasses[i].style)
            {
                cam.cullingMask = styleClasses[i].layerMask;
            }
        }
    }

    private void CalculatePresentStyle()
    {
        if (Styles.Count == 0)
        {
            return;
        }
        string[] enumArray = Enum.GetNames(typeof(Style));
        int[] StyleVoteNumbers = new int[enumArray.Length];

        for (int i = 0; i < enumArray.Length;i++)
        {
            for (int j = 0; j < Styles.Count; j++)
            {
                if (Styles[j] == (Style)i)
                {
                    StyleVoteNumbers[i] ++;
                }
            }
        }

        int x = 0;
        int y = 0;
        for (int i = 0; i < StyleVoteNumbers.Length;i++)
        {
            if (StyleVoteNumbers[i] > x)
            {
                x = StyleVoteNumbers[i];
                y = i;
            }
        }
        presentStyle = (Style)y;
    }

    private void Start()
    {
        CalculatePresentStyle();
    }
}

[Serializable]
public class StyleClass
{
    public GameBrainScript.Style style;
    public LayerMask layerMask;
}