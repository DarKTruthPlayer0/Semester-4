using UnityEngine;
public class CamMove : MonoBehaviour
{
    [SerializeField] [Range(0,100)] private float camMovementSlowdownFactor;
    [SerializeField] private Vector2 maxMoveDistance;
    private Vector3 startPos;
    private Vector2 directionVec;
    private void CheckCamMoveDirection()
    {
        if (gameObject.transform.position.x < (startPos.x - maxMoveDistance.x) && directionVec.x < 0)
        {
            // Arrow down off
            directionVec.x = 0;
        }
        if (gameObject.transform.position.x > (startPos.x + maxMoveDistance.x) && directionVec.x > 0)
        {
            // Arrow up off
            directionVec.x = 0;
        }
        if (gameObject.transform.position.y < (startPos.y - maxMoveDistance.y) && directionVec.y < 0)
        {
            // Arrow left off
            directionVec.y = 0;
        }
        if (gameObject.transform.position.y > (startPos.y + maxMoveDistance.y) && directionVec.y > 0)
        {
            // Arrow right off;
            directionVec.y = 0;

        }
    }

    private void Update()
    {
        directionVec.x = Input.GetAxis("Mouse X");
        directionVec.y = Input.GetAxis("Mouse Y");
        CheckCamMoveDirection();
        directionVec *= (1/camMovementSlowdownFactor);
        gameObject.transform.position = new Vector3(transform.position.x + directionVec.x, transform.position.y + directionVec.y, transform.position.z);
    }
    private void Start()
    {
        startPos = gameObject.transform.position;
    }
}
