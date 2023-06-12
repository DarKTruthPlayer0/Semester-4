using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    [SerializeField] private string tagPingPongBall;
    
    [Range(0, 10)] public float ballSpeed;

    [SerializeField] private int scoreToReach;
    private Rigidbody rb;
    private Score score;

    private bool CheckScore()
    {
        if (score.PlayerScore >= scoreToReach)
        {
            //Player wins
            return true;
        }
        else if (score.NPCScore < scoreToReach)
        {
            //NPC wins
            return true;
        }
        return false;
    }

    private void NewRound()
    {
        rb.velocity = Vector3.right * ballSpeed;
    }


    private void Update()
    {
        if (Input.anyKeyDown)
        {
            NewRound();
        }
    }

    private void Start()
    {
        rb = GameObject.FindGameObjectWithTag(tagPingPongBall).GetComponent<Rigidbody>();
    }


}

[Serializable]
public class Score
{
    public int PlayerScore;
    public int NPCScore;
}
