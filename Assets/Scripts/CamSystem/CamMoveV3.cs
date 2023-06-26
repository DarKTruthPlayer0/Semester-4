using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CamMoveV3 : MonoBehaviour
{
    [HideInInspector] public bool MoveToDirection;
    [HideInInspector] public Directions MoveDirection;

    [SerializeField] private GameObject cam;
    [SerializeField] private float camMoveSpeed;
    [SerializeField] private float camSmoothTime;
    [SerializeField] private float maxCamMoveSpeed;
    private Vector3 camTargetPosition;
    private Vector3 currentCamMoveVelociy;
    private bool breakForLoop;

    [SerializeField] private string tagBorders;
    [SerializeField] private Border[] borders = new Border[Enum.GetNames(typeof(Directions)).Length];

    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;


    private void CalculateNewCamTargetPosition()
    {

        switch (MoveDirection)
        {
            case Directions.Right:
                print("Right");
                if (camTargetPosition.x + (Time.deltaTime * camMoveSpeed) < BorderValue())
                {
                    camTargetPosition.x += Time.deltaTime * camMoveSpeed;
                    if (!leftArrow.activeInHierarchy)
                    {
                        leftArrow.SetActive(true);
                    }
                }
                else
                {
                    camTargetPosition.x = BorderValue();
                    rightArrow.SetActive(false);
                    MoveToDirection = false;
                }
                break;
            case Directions.Left:
                print("Left");
                if (camTargetPosition.x - (Time.deltaTime * camMoveSpeed) > BorderValue())
                {
                    camTargetPosition.x -= Time.deltaTime * camMoveSpeed;
                    if (!rightArrow.activeInHierarchy) 
                    { 
                        rightArrow.SetActive(true); 
                    }
                }
                else
                {
                    camTargetPosition.x = BorderValue();
                    leftArrow.SetActive(false);
                    MoveToDirection = false;
                }
                break;
            case Directions.Up:
                print("Up");
                if (camTargetPosition.y + (Time.deltaTime * camMoveSpeed) < BorderValue())
                {
                    camTargetPosition.y += Time.deltaTime * camMoveSpeed;
                    if (!downArrow.activeInHierarchy) 
                    { 
                        downArrow.SetActive(true); 
                    }
                }
                else
                {
                    camTargetPosition.y = BorderValue();
                    upArrow.SetActive(false);
                    MoveToDirection = false;
                }
                break;
            case Directions.Down:
                print("Down");
                if (camTargetPosition.y - (Time.deltaTime * camMoveSpeed) > BorderValue())
                {
                    camTargetPosition.y -= Time.deltaTime * camMoveSpeed;
                    if (!upArrow.activeInHierarchy) 
                    { 
                        upArrow.SetActive(true);
                    }
                }
                else
                {
                    camTargetPosition.y = BorderValue();
                    downArrow.SetActive(false);
                    MoveToDirection = false;
                }
                break;
        }

    }

    private float BorderValue()
    {
        float tmpBorderValue = 0;

        for (int i = 0; i < borders.Length; i++)
        {
            if (MoveDirection != borders[i].DirectionBorder)
            {
                continue;
            }
            switch (MoveDirection)
            {
                case Directions.Right:
                    breakForLoop = true;
                    tmpBorderValue = borders[i].BorderObject.transform.position.x;
                    break;
                case Directions.Left:
                    breakForLoop = true;
                    tmpBorderValue = borders[i].BorderObject.transform.position.x;
                    break;
                case Directions.Up:
                    breakForLoop = true;
                    tmpBorderValue = borders[i].BorderObject.transform.position.y;
                    break;
                case Directions.Down:
                    breakForLoop = true;
                    tmpBorderValue = borders[i].BorderObject.transform.position.y;
                    break;
            }
            if (breakForLoop)
            {
                break;
            }
        }
        breakForLoop = false;
        return tmpBorderValue;
    }

    private void SetCamToPosition()
    {
        cam.transform.position = Vector3.SmoothDamp(cam.gameObject.transform.position, camTargetPosition, ref currentCamMoveVelociy, camSmoothTime, maxCamMoveSpeed, Time.deltaTime);
    }


    public void SetBorderGOs()
    {
        // Entfernen aller überflüssigen GOs
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            if (!gameObject.transform.GetChild(i).gameObject.CompareTag(tagBorders) || i > borders.Length - 1)
            {
                DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            }
        }

        // Hinzufügen fehlender GOs
        if (gameObject.transform.childCount < borders.Length)
        {
            int tmpInstantiateNumber = borders.Length - gameObject.transform.childCount;

            for (int i = 0; i < tmpInstantiateNumber; i++)
            {
                GameObject newObj = new GameObject("Border");
                newObj.tag = tagBorders;
                newObj.transform.SetParent(gameObject.transform);
            }
        }

        // Setzen der Border-Namen + einfügen in BorderArray
        string[] tmpBorderNameAdd = Enum.GetNames(typeof(Directions));
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.name = tmpBorderNameAdd[i] + "Border";

            borders[i].BorderObject = gameObject.transform.GetChild(i).gameObject;
            borders[i].DirectionBorder = (Directions)i;
        }
    }

    private void Start()
    {
        camTargetPosition = gameObject.transform.position;

        SetBorderGOs();

        rightArrow = GameObject.Find("RightArrow");
        leftArrow = GameObject.Find("LeftArrow");
        upArrow = GameObject.Find("UpArrow");
        downArrow = GameObject.Find("DownArrow");

        cam = gameObject.transform.parent.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (MoveToDirection)
        {
            CalculateNewCamTargetPosition();
        }
        SetCamToPosition();
    }
}

[Serializable]
public class Border
{
    public GameObject BorderObject;
    public Directions DirectionBorder;
}

public enum Directions
{
    Right,
    Left,
    Up,
    Down
}
