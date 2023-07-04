using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameLogic : MonoBehaviour
{
    public string TagPingPongBall;

    [SerializeField] private string tagPlayerDeathzone;
    [SerializeField] private string tagNPCDeathzone;
    
    [SerializeField] [Range(0, 100)] private float ballSpeed;

    [SerializeField] private int scoreToReach;

    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text nPCScoreText;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text WinScreenText;

    private Vector3 startPos;
    private Rigidbody ballRb;
    private Score score = new();
    private NPCBrain nPCBrain;
    private bool gameRunning;

    public void SetScore(DeathZone deathZone)
    {
        if (deathZone == DeathZone.PlayerDeathZone)
        {
            score.NPCScore++;
            nPCScoreText.text = score.NPCScore.ToString();
        }
        if (deathZone == DeathZone.NPCDeathZone)
        {
            score.PlayerScore++;
            playerScoreText.text = score.PlayerScore.ToString();
        }
        ResetBall();
        CheckScore();
    }

    private void ResetBall()
    {
        gameRunning = false;
        nPCBrain.TargetHeigth = 0;
        ballRb.velocity = Vector3.zero;
        ballRb.gameObject.transform.position = startPos;
    }

    private bool CheckScore()
    {
        if (score.PlayerScore >= scoreToReach)
        {
            //Player wins
            print("Player Wins");
            if (!winScreen.activeInHierarchy)
            {
                winScreen.SetActive(true);
            }
            return true;
        }
        if (score.NPCScore >= scoreToReach)
        {
            //NPC wins
            print("NPC Wins");
            if (!winScreen.activeInHierarchy)
            {
                winScreen.SetActive(true);
            }
            return true;
        }
        return false;
    }

    private void StartRound()
    {
        Vector3 tmpStartVec;
        tmpStartVec.x = 1;
        tmpStartVec.y = Random.Range(-1f, 1f);
        tmpStartVec.z = 0;
        tmpStartVec = tmpStartVec.normalized;
        ballRb.velocity = tmpStartVec * ballSpeed;
    }

    private void SetUpDeathZones()
    {
        GameObject tmpPlayerDeathZone = GameObject.FindGameObjectWithTag(tagPlayerDeathzone);
        DeathZoneTriggerScript tmpPlayerDeathZoneTriggerScript = tmpPlayerDeathZone.AddComponent<DeathZoneTriggerScript>();
        tmpPlayerDeathZoneTriggerScript.MainLogic = this;
        tmpPlayerDeathZoneTriggerScript.WhichDeathZone = DeathZone.PlayerDeathZone;

        GameObject tmpNPCDeathZone = GameObject.FindGameObjectWithTag(tagNPCDeathzone);
        DeathZoneTriggerScript tmpNPCDeathZoneTriggerScript = tmpNPCDeathZone.AddComponent<DeathZoneTriggerScript>();
        tmpNPCDeathZoneTriggerScript.MainLogic = this;
        tmpNPCDeathZoneTriggerScript.WhichDeathZone = DeathZone.NPCDeathZone;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !gameRunning && !CheckScore())
        {
            StartRound();
            gameRunning = true;
        }
    }

    private void Start()
    {
        ballRb = GameObject.FindGameObjectWithTag(TagPingPongBall).GetComponent<Rigidbody>();

        SetUpDeathZones();

        startPos = ballRb.gameObject.transform.position;

        nPCBrain = GetComponent<NPCBrain>();
    }


}
public enum DeathZone
{
    PlayerDeathZone,
    NPCDeathZone
}

[System.Serializable]
public class Score
{
    public int PlayerScore;
    public int NPCScore;
}

public class DeathZoneTriggerScript : MonoBehaviour
{
    public MiniGameLogic MainLogic;
    public DeathZone WhichDeathZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(MainLogic.TagPingPongBall))
        {
            MainLogic.SetScore(WhichDeathZone);
        }
    }
}
