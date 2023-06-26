using UnityEngine;

public class CamMoveTranslate : MonoBehaviour
{
    private CamMoveV3[] camMoveV3s;

    public void SetCamMove(bool MoveToDirectionTranslate, Directions direction)
    {
        for (int i = 0; i < camMoveV3s.Length; i++)
        {
            if (!camMoveV3s[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            camMoveV3s[i].MoveToDirection = MoveToDirectionTranslate;
            camMoveV3s[i].MoveDirection = direction;
        }
    }

    private void Start()
    {
        camMoveV3s = FindObjectsOfType<CamMoveV3>();
    }
}
