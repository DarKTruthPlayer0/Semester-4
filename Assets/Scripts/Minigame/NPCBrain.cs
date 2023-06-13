using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    private Rigidbody rb;

    private float timeToCollision;
    private Vector3 ballPos;

    // Entfernung gilt ab StartPosition
    // Entfernung zum LevelRand Vertikal ist +/-40,25
    // Entfernung zur Bar Horizontal ist +/-65,5

    private void CalculateBallMovement()
    {
        timeToCollision = (65.5f * 2) / rb.velocity.x;

        float tmpYVelo = rb.velocity.y;
        
    }
}
