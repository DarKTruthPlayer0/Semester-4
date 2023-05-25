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
public class CamMoveV2 : ListFunctionsExtension
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

        ListCompare(camPositions, tmpPositionGOsList, () => new CamPositions());

        for (int i = 0; i < camPositions.Count; i++)
        {
            ListCompareListsUseSameGOs(camPositions[i].PossibleCamPositions, tmpPositionGOsList, camPositions[i].ActualPositionGO, () => new CamPosition());
        }
    }

    private void Awake()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        cam = gameObject.transform.parent.GetChild(0).gameObject;

        rightArrow = GameObject.Find("RightArrow");
        leftArrow = GameObject.Find("LeftArrow");
        upArrow = GameObject.Find("UpArrow");
        downArrow = GameObject.Find("DownArrow");
    }
    private void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }
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
public class CamPositions : Translate
{
    public GameObject ActualPositionGO;
    public List<CamPosition> PossibleCamPositions = new();

    public GameObject GOTranslate
    {
        get { return ActualPositionGO; }
        set { ActualPositionGO = value; }
    }
}

[Serializable]
public class CamPosition : Translate
{
    public GameObject PossiblePositionGO;
    public bool CanMoveTo;
    public Directions Direction;

    public GameObject GOTranslate
    {
        get { return PossiblePositionGO; }
        set { PossiblePositionGO = value; }
    }
}
