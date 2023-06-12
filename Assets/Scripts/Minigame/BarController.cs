using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BarController : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float smoothTime;
    [SerializeField] private float maxSpeedPlayerBar;
    private GameObject playerBar;
    private GameObject nPCBar;
    private Vector3 targetVec;
    private Vector3 currentPlayerBarVelocity;

    private void MovePlayerBar()
    {
        targetVec.x = playerBar.transform.position.x;
        targetVec.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        targetVec.z = playerBar.transform.position.z;


        if (playerBar.transform.position.y >= moveDistance && targetVec.y > moveDistance)
        {
            targetVec.y = moveDistance;
        }
        if (playerBar.transform.position.y <= -moveDistance && targetVec.y < -moveDistance)
        {
            targetVec.y = -moveDistance;
        }

        Vector3.SmoothDamp(playerBar.gameObject.transform.position, targetVec, ref currentPlayerBarVelocity, smoothTime, maxSpeedPlayerBar, Time.deltaTime);
    }

    private void Update()
    {
        
    }
}
