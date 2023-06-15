using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BarController : MonoBehaviour
{
    [SerializeField] private string tagPlayerBar;
    [SerializeField] private string tagNPCBar;
    [SerializeField] private string tagPingPongBall;

    [SerializeField] private float moveDistance;
    [SerializeField] private float smoothTime;
    [SerializeField] private float maxSpeedPlayerBar;
    [SerializeField] private float maxSpeedNPCBar;
    private GameObject playerBar;
    private GameObject nPCBar;
    private Vector3 currentPlayerBarVelocity;
    private Vector3 currentNPCBarVelocity;
    private NPCBrain nPCBrain;
    private Vector3 targetVecPlayerBar;
    private Vector3 targetVecNPCBar;

    private void MovePlayerBar()
    {
        targetVecPlayerBar.x = playerBar.transform.position.x;
        targetVecPlayerBar.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        targetVecPlayerBar.z = playerBar.transform.position.z;


        if (playerBar.transform.position.y >= moveDistance && targetVecPlayerBar.y > moveDistance)
        {
            targetVecPlayerBar.y = moveDistance;
        }
        if (playerBar.transform.position.y <= -moveDistance && targetVecPlayerBar.y < -moveDistance)
        {
            targetVecPlayerBar.y = -moveDistance;
        }

        playerBar.transform.position = Vector3.SmoothDamp(playerBar.gameObject.transform.position, targetVecPlayerBar, ref currentPlayerBarVelocity, smoothTime, maxSpeedPlayerBar, Time.deltaTime);
    }

    private void MoveNPCBar()
    {
        targetVecNPCBar.x = nPCBar.transform.position.x;
        targetVecNPCBar.y = nPCBrain.TargetHeigth;
        targetVecNPCBar.z = nPCBar.transform.position.z;


        if (nPCBar.transform.position.y >= moveDistance && targetVecNPCBar.y > moveDistance)
        {
            targetVecNPCBar.y = moveDistance;
        }
        if (nPCBar.transform.position.y <= -moveDistance && targetVecNPCBar.y < -moveDistance)
        {
            targetVecNPCBar.y = -moveDistance;
        }

        nPCBar.transform.position = Vector3.SmoothDamp(nPCBar.gameObject.transform.position, targetVecNPCBar, ref currentNPCBarVelocity, smoothTime, maxSpeedNPCBar, Time.deltaTime);

    }

    private void Start()
    {
        nPCBrain = GetComponent<NPCBrain>();
        nPCBar = GameObject.FindGameObjectWithTag(tagNPCBar);
        playerBar = GameObject.FindGameObjectWithTag(tagPlayerBar);
        TriggerBallPosCalculation tmpTBPC = playerBar.AddComponent<TriggerBallPosCalculation>();
        tmpTBPC.nPCBrain = nPCBrain;
        tmpTBPC.TagPingPongBall = tagPingPongBall;
    }

    private void Update()
    {
        MovePlayerBar();
        MoveNPCBar();
    }
}

public class TriggerBallPosCalculation : MonoBehaviour
{
    public NPCBrain nPCBrain;
    public string TagPingPongBall;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagPingPongBall))
        {
            nPCBrain.TriggerBallPosCalculation();
        }
    }
}
