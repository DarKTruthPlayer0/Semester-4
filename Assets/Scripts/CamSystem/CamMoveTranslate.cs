using UnityEngine;

public class CamMoveTranslate : MonoBehaviour
{
    private CamMoveV2[] camMoveV2s;

    public void TranslateRight()
    {
        foreach (CamMoveV2 cMV2 in camMoveV2s)
        {
            if (cMV2.transform.parent.gameObject.activeInHierarchy)
            {
                cMV2.MoveRight();
            }
        }
    }
    public void TranslateLeft()
    {
        foreach (CamMoveV2 cMV2 in camMoveV2s)
        {
            if (cMV2.transform.parent.gameObject.activeInHierarchy)
            {
                cMV2.MoveLeft();
            }
        }
    }
    public void TranslateUp()
    {
        foreach (CamMoveV2 cMV2 in camMoveV2s)
        {
            if (cMV2.transform.parent.gameObject.activeInHierarchy)
            {
                cMV2.MoveUp();
            }
        }
    }
    public void TranslateDown()
    {
        foreach (CamMoveV2 cMV2 in camMoveV2s)
        {
            if (cMV2.transform.parent.gameObject.activeInHierarchy)
            {
                cMV2.MoveDown();
            }
        }
    }

    private void Start()
    {
        camMoveV2s = FindObjectsOfType<CamMoveV2>();
    }
}
