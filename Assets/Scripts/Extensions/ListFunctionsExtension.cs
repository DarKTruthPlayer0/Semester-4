using System.Collections.Generic;
using UnityEngine;

public class Kuchen
{
    public void KuchenFunktion<T>(List<T> kr�melListe, List<GameObject> tmpKr�melGOsVergleich) where T : IKr�mel
    {
        for (int i = 0; i < kr�melListe.Count; i++)
        {
            Kr�mel(i, kr�melListe, tmpKr�melGOsVergleich);
        }
    }

    void Kr�mel<T>(int i, List<T> kr�melListe, List<GameObject> tmpKr�melGOsVergleich) where T : IKr�mel
    {
        for (int j = 0; j < tmpKr�melGOsVergleich.Count; j++)
        {
            if (kr�melListe[i].Kr�melGO == tmpKr�melGOsVergleich[j])
            {
                // Hier k�nnen Sie Code hinzuf�gen, der ausgef�hrt wird,
                // wenn das GameObject "Kr�melGO" einem der GameObjects
                // in der Liste "tmpKr�melGOsVergleich" entspricht.
            }
        }
    }
}

public interface IKr�mel
{
    GameObject Kr�melGO { get; set; }
}

public class Torte : IKr�mel
{
    public GameObject TortenGO;
    public bool Existiert;
    public string TortenName;

    public GameObject Kr�melGO
    {
        get { return TortenGO; }
        set { TortenGO = value; }
    }
}