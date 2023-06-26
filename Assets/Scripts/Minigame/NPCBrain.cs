using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    [HideInInspector] public float TargetHeigth = 0;
    private float tmpTargetheigth;

    [SerializeField] private string tagPingPongBall;
    [SerializeField][Range(0, 1)] private float waitforCalculationTime;
    private Rigidbody rb;

    private float timeXDirection;
    private int tmpVorzeichen;
    private Vector3 ballPos;
    private float bounces;

    // Entfernung gilt ab StartPosition
    // Entfernung zum LevelRand Vertikal ist +/-40,25
    // Entfernung zur Bar Horizontal ist +/-65,5

    public void TriggerBallPosCalculation()
    {
        StartCoroutine(WaitforCalculation());
        
    }

    private IEnumerator WaitforCalculation()
    {
        yield return new WaitForSeconds(waitforCalculationTime);
        CalculateBallMovement();
    }

    public void CalculateBallMovement()
    {
        TargetHeigth = 0;
        ballPos = rb.gameObject.transform.position;
        timeXDirection = (ballPos.x + 65.5f) / Mathf.Abs(rb.velocity.x);
        float alpha = Mathf.Abs(rb.velocity.y) / Mathf.Abs(rb.velocity.x);
        bounces = 0;
        
        // Prevent all uneccesary Errors and Calculations
        if (alpha == float.NaN)
        {
            TargetHeigth = 0;
            return;
        }
        if (rb.velocity.x > 0 || rb.velocity.y == 0)
        {
            TargetHeigth = 0;
            return;
        }

        // Ermitteln ob Bumps
        float tmpTimeXDirection = timeXDirection;
        if (rb.velocity.y < 0)
        {
            if (ballPos.y < 0)
            {
                tmpTimeXDirection -= ((40.25f - Mathf.Abs(ballPos.y)) / alpha) / Mathf.Abs(rb.velocity.x);
            }
            else
            {
                tmpTimeXDirection -= ((40.25f + Mathf.Abs(ballPos.y)) / alpha) / Mathf.Abs(rb.velocity.x);
            }
        }
        else
        {
            if (ballPos.y < 0)
            {
                tmpTimeXDirection -= ((40.25f + Mathf.Abs(ballPos.y)) / alpha) / Mathf.Abs(rb.velocity.x);
            }
            else
            {
                tmpTimeXDirection -= ((40.25f - Mathf.Abs(ballPos.y)) / alpha) / Mathf.Abs(rb.velocity.x);
            }
        }

        if (tmpTimeXDirection > 0)
        {
            bounces += 1;
            float bumpTimeY = ((40.25f * 2) / alpha) / Mathf.Abs(rb.velocity.x);
            for (; tmpTimeXDirection > bumpTimeY; tmpTimeXDirection -= bumpTimeY)
            {
                bounces++;
            }
        }
        //Ermitteln der Targetheigth no Bumps
        else
        {
            float tmpOvertime = Mathf.Abs(tmpTimeXDirection);
            float tmpTargetheigth = 0;

            tmpTargetheigth = (tmpOvertime * Mathf.Abs(rb.velocity.x)) * alpha;
            if (rb.velocity.y < 0)
            {
                TargetHeigth = -40.25f + tmpTargetheigth;
            }
            else
            {
                TargetHeigth = 40.25f - tmpTargetheigth;
            }
            return;
        }

        bounces += (Mathf.Abs(rb.velocity.y) * tmpTimeXDirection) / (40.25f * 2);


        // Ermitteln des Vorzeichens
        if (rb.velocity.y < 0)
        {
            tmpVorzeichen = -1;
        }
        else
        {
            tmpVorzeichen = 1;
        }

        for (; 1 < Mathf.Abs(bounces);)
        {
            tmpVorzeichen *= -1;
            if (bounces < 0)
            {
                bounces += 1;
            }
            else
            {
                bounces -= 1;
            }
        }

        // Ermitteln der Targetheigth mit Bumps
        tmpTargetheigth = (40.25f * 2) * bounces;

        if (tmpVorzeichen < 0)
        {
            TargetHeigth = 40.25f - tmpTargetheigth;
        }
        else
        {
            TargetHeigth = -40.25f + tmpTargetheigth;
        }
    }

    private void Start()
    {
        GameObject tmpGO = GameObject.FindGameObjectWithTag(tagPingPongBall);
        rb = tmpGO.GetComponent<Rigidbody>();
    }
}