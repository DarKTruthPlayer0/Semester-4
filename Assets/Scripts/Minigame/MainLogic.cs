using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public string TagPingPongBall;

    [SerializeField] private string tagPlayerDeathzone;
    [SerializeField] private string tagNPCDeathzone;
    
    [SerializeField] [Range(0, 100)] private float ballSpeed;

    [SerializeField] private int scoreToReach;
    private Rigidbody ballRb;
    private Score score;
    private bool gameRunning;

    public void SetScore(DeathZone deathZone)
    {
        if (deathZone == DeathZone.PlayerDeathZone)
        {
            score.PlayerScore++;
        }
        if (deathZone == DeathZone.NPCDeathZone)
        {
            score.NPCScore++;
        }
        CheckScore();
    }

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

    private void StartRound()
    {
        ballRb.velocity = Vector3.right * ballSpeed;
    }


    private void Update()
    {
        if (Input.anyKeyDown && !gameRunning)
        {
            StartRound();
            gameRunning = true;
        }
    }

    private void Start()
    {
        ballRb = GameObject.FindGameObjectWithTag(TagPingPongBall).GetComponent<Rigidbody>();

        GameObject tmpPlayerDeathZone = GameObject.FindGameObjectWithTag(tagPlayerDeathzone);
        DeathZoneTriggerScript tmpPlayerDeathZoneTriggerScript = tmpPlayerDeathZone.AddComponent<DeathZoneTriggerScript>();
        tmpPlayerDeathZoneTriggerScript.MainLogic = this;
        tmpPlayerDeathZoneTriggerScript.WhichDeathZone = DeathZone.PlayerDeathZone;

        GameObject tmpNPCDeathZone = GameObject.FindGameObjectWithTag(tagNPCDeathzone);
        DeathZoneTriggerScript tmpNPCDeathZoneTriggerScript = tmpNPCDeathZone.AddComponent<DeathZoneTriggerScript>();
        tmpNPCDeathZoneTriggerScript.MainLogic = this;
        tmpNPCDeathZoneTriggerScript.WhichDeathZone = DeathZone.NPCDeathZone;
    }


}
public enum DeathZone
{
    PlayerDeathZone,
    NPCDeathZone
}

[Serializable]
public class Score
{
    public int PlayerScore;
    public int NPCScore;
}

public class DeathZoneTriggerScript : MonoBehaviour
{
    public MainLogic MainLogic;
    public DeathZone WhichDeathZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(MainLogic.TagPingPongBall))
        MainLogic.SetScore(WhichDeathZone);
    }
}
