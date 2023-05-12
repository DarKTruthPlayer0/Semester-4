using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Directions
{
    Right,
    Left,
    Up,
    Down
}
[ExecuteInEditMode]
public class CamMoveV2 : MonoBehaviour
{
    [Header("Assing")]
    [SerializeField] private string tagPositions;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject cam;

    public List<CamPositions> camPositions;
    private int speedupInt;
    private bool sortHelpBool;
    private bool camMoved;

    public void MoveRight()
    {
        CamMove(Directions.Right);
    }
    public void MoveLeft()
    {
        CamMove(Directions.Left);
    }
    public void MoveUp()
    {
        CamMove(Directions.Up);
    }
    public void MoveDown()
    {
        CamMove(Directions.Down);
    }

    private void CamMove(Directions direction)
    {
        for (int i = 0; i < camPositions.Count; i++)
        {
            if (camPositions[i].ActualPositionGO.transform.position != cam.transform.position)
            {
                continue;
            }
            for (int j = 0; j < camPositions[i].PossibleCamPositions.Count; j++)
            {
                if (direction == camPositions[i].PossibleCamPositions[j].Direction && camPositions[i].PossibleCamPositions[j].CanMoveTo)
                {
                    cam.transform.position = camPositions[i].PossibleCamPositions[j].PossiblePositionGO.transform.position;
                    camMoved = true;
                    break;
                }
            }
            if (camMoved)
            {
                camMoved = false;
                break;
            }
        }

        DeactivateArrowsIfNecessary();
    }

    private void DeactivateArrowsIfNecessary()
    {
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
        upArrow.SetActive(false);
        downArrow.SetActive(false);

        for (int i = 0; i < camPositions.Count; i++)
        {
            if (camPositions[i].ActualPositionGO.transform.position.Equals(cam.transform.position))
            {
                speedupInt = i;
            }
        }

        for (int i = 0; i < camPositions[speedupInt].PossibleCamPositions.Count; i++)
        {
            if (!camPositions[speedupInt].PossibleCamPositions[i].CanMoveTo)
            {
                continue;
            }
            switch (camPositions[speedupInt].PossibleCamPositions[i].Direction)
            {
                case Directions.Right:
                    rightArrow.SetActive(true);
                    continue;
                case Directions.Left:
                    leftArrow.SetActive(true);
                    continue;
                case Directions.Up:
                    upArrow.SetActive(true);
                    continue;
                case Directions.Down:
                    downArrow.SetActive(true);
                    continue;
            }
        }
    }

    private void CamPositionsOrganize()
    {
        List<GameObject> tmpPositionGOsList = new();
        Transform[] tmpPositionTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in tmpPositionTransforms)
        {
            if (t.CompareTag(tagPositions))
            {
                tmpPositionGOsList.Add(t.gameObject);
            }
        }
        GameObject[] tmpPositionGOs = tmpPositionGOsList.ToArray();

        for (int i = 0; i < camPositions.Count; i++)
        {
            for (int j = 0; j < tmpPositionGOs.Length; j++)
            {
                if (camPositions[i].ActualPositionGO == tmpPositionGOs[j])
                {
                    sortHelpBool = true;
                    break;
                }
                else
                {
                    sortHelpBool = false;
                }
            }
            if (sortHelpBool)
            {
                continue;
            }
            camPositions.RemoveAt(i);
        }
        for (int j = 0; j < tmpPositionGOs.Length; j++)
        {
            for (int k = 0; k < camPositions.Count; k++)
            {
                if (camPositions[k].ActualPositionGO == tmpPositionGOs[j] && tmpPositionGOs[k])
                {
                    sortHelpBool = true;
                    break;
                }
                else
                {
                    sortHelpBool = false;
                }
            }
            if (sortHelpBool)
            {
                continue;
            }
            CamPositions tmpCamPositions = new()
            {
                ActualPositionGO = tmpPositionGOs[j]
            };
            camPositions.Add(tmpCamPositions);
        }


        for (int i = 0; i < camPositions.Count; i++)
        {
            for (int j = 0; j < camPositions[i].PossibleCamPositions.Count; j++)
            {
                for (int k = 0; k < tmpPositionGOs.Length; k++)
                {
                    if (camPositions[i].PossibleCamPositions[j].PossiblePositionGO == tmpPositionGOs[k])
                    {
                        sortHelpBool = true;
                        break;
                    }
                    else
                    {
                        sortHelpBool = false;
                    }
                }
                if (sortHelpBool)
                {
                    continue;
                }
                camPositions[i].PossibleCamPositions.RemoveAt(j);
            }

            for (int j = 0; j < tmpPositionGOs.Length; j++)
            {
                for (int k = 0; k < camPositions[i].PossibleCamPositions.Count; k++)
                {
                    if (camPositions[i].PossibleCamPositions[k].PossiblePositionGO == tmpPositionGOs[j])
                    {
                        sortHelpBool = true;
                        break;
                    }
                    else
                    {
                        sortHelpBool = false;
                    }
                }
                if (sortHelpBool || tmpPositionGOs[j] == camPositions[i].ActualPositionGO)
                {
                    continue;
                }
                CamPosition tmpCamPos = new()
                {
                    PossiblePositionGO = tmpPositionGOs[j]
                };
                camPositions[i].PossibleCamPositions.Add(tmpCamPos);
            }
        }
    }

    private void Awake()
    {
        cam = gameObject.transform.parent.GetChild(0).gameObject;

        rightArrow = GameObject.Find("RightArrow");
        leftArrow = GameObject.Find("LeftArrow");
        upArrow = GameObject.Find("UpArrow");
        downArrow = GameObject.Find("DownArrow");
    }
    private void Start()
    {
        DeactivateArrowsIfNecessary();
    }
    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        if (!rightArrow.activeInHierarchy)
        {
            rightArrow.SetActive(true);
        }
        if (!leftArrow.activeInHierarchy)
        {
            leftArrow.SetActive(true);
        }
        if (!upArrow.activeInHierarchy)
        {
            upArrow.SetActive(true);
        }
        if (!downArrow.activeInHierarchy)
        {
            downArrow.SetActive(true);
        }

        CamPositionsOrganize();
    }
}

[Serializable]
public class CamPositions
{
    public GameObject ActualPositionGO;
    public List<CamPosition> PossibleCamPositions = new();
}

[Serializable]
public class CamPosition
{
    public GameObject PossiblePositionGO;
    public bool CanMoveTo;
    public Directions Direction;
}
