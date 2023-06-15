using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    [HideInInspector] public float TargetHeigth;
    private float tmpTargetheigth;

    [SerializeField] private string tagPingPongBall;
    [SerializeField][Range(0, 1)] private float waitforCalculationTime;
    private Rigidbody rb;
    private SphereCollider sphereCollider;

    private float timeToCollision;
    private Vector3 ballPos;

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
        timeToCollision = (65.5f * 2 / Mathf.Abs(rb.velocity.x));
        float alpha = Mathf.Atan(Mathf.Abs(rb.velocity.y) / Mathf.Abs(rb.velocity.x));

        print("Alpha: " + alpha);

        // Berechnung der anzahl der Abpraller
        float bounces = ((Mathf.Abs(rb.velocity.y) * timeToCollision) - Mathf.Abs(ballPos.y)) / (40.25f * 2);


        float length = 65.5f * 2;

        float tmpBounces = bounces;
        if (tmpBounces >= 1)
        {

            // Abzug der Strecke bis ersten Abpraller
            if (rb.velocity.y < 0)
            {
                if (ballPos.y < 0)
                {
                    length -= (40.25f - ballPos.y) / alpha;
                }
                else
                {
                    length -= (40.25f + ballPos.y) / alpha;
                }
            }
            else
            {
                if (ballPos.y < 0)
                {
                    length -= (40.25f + ballPos.y) / alpha;
                }
                else
                {
                    length -= (40.25f - ballPos.y) / alpha;
                }
            }
            tmpBounces -= 1;


            // Berechnung der Strecke für jeden Abpraller
            for (float i = tmpBounces; i > 1; i--)
            {
                length -= (40.25f * 2) / alpha;
            }

            tmpTargetheigth = alpha * length;
        }


        // Ermitteln des Vorzeichens
        if (rb.velocity.y < 0)
        {
            bounces *= -1;
        }
        print("Bounces: " + bounces);
        for (; Mathf.Abs(bounces) >= 1;)
        {
            bounces *= -1;
            if (bounces < 0)
            {
                bounces += 1;
            }
            else
            {
                bounces -= 1;
            }
        }

        // Setzen von targetHeigth
        print(tmpTargetheigth);
        if (bounces < 0)
        {
            TargetHeigth = 40.25f - tmpTargetheigth;
        }
        else
        {
            TargetHeigth = - 40.25f + tmpTargetheigth;
        }


        print("Targetheigth: " + TargetHeigth);
        Debug.Break();
    }

    private void Start()
    {
        GameObject tmpGO = GameObject.FindGameObjectWithTag(tagPingPongBall);
        rb = tmpGO.GetComponent<Rigidbody>();
        sphereCollider = tmpGO.GetComponent<SphereCollider>();
    }
}