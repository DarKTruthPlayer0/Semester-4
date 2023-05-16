using System.Collections.Generic;
using UnityEngine;

public class Kuchen
{
    public void KuchenFunktion<T>(List<T> krümelListe, List<GameObject> tmpKrümelGOsVergleich) where T : IKrümel
    {
        for (int i = 0; i < krümelListe.Count; i++)
        {
            Krümel(i, krümelListe, tmpKrümelGOsVergleich);
        }
    }

    void Krümel<T>(int i, List<T> krümelListe, List<GameObject> tmpKrümelGOsVergleich) where T : IKrümel
    {
        for (int j = 0; j < tmpKrümelGOsVergleich.Count; j++)
        {
            if (krümelListe[i].KrümelGO == tmpKrümelGOsVergleich[j])
            {
                // Hier können Sie Code hinzufügen, der ausgeführt wird,
                // wenn das GameObject "KrümelGO" einem der GameObjects
                // in der Liste "tmpKrümelGOsVergleich" entspricht.
            }
        }
    }
}

public interface IKrümel
{
    GameObject KrümelGO { get; set; }
}

public class Torte : IKrümel
{
    public GameObject TortenGO;
    public bool Existiert;
    public string TortenName;

    public GameObject KrümelGO
    {
        get { return TortenGO; }
        set { TortenGO = value; }
    }
}