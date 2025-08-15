using System.Collections;
using UnityEngine;
public class WaterRapids : MonoBehaviour
{
    [Tooltip("Where the ball starts being moved.")]
    public Transform startPoint;

    [Tooltip("Where the ball should be moved to.")]
    public Transform endPoint;

    [Tooltip("Speed at which the conveyor moves the ball.")]
    public float conveyorSpeed = 3f;

    [Tooltip("Reference to the next WaterRapids in the sequence.")]
    public WaterRapids nextRapids;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteBall") || other.CompareTag("BlackBall"))
        {
            Debug.Log($"[Rapids] Triggered by {other.name}");
            StartCoroutine(MoveAlongConveyor(other.gameObject, false));
        }
    }

    private IEnumerator MoveAlongConveyor(GameObject ball, bool forceStartAtPoint)
    {
        // Disable character control
        CharacterMovement cm = ball.GetComponent<CharacterMovement>();
        if (cm != null)
            cm.enabled = false;

        Debug.Log(cm.enabled);

        yield return new WaitForSeconds(0.2f);

        if (forceStartAtPoint)
        {
            // Snap ball to startPoint before moving
            ball.transform.position = startPoint.position;
        }

        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;
        Vector3 direction = (end - start).normalized;

        Debug.Log($"[Rapids] Moving {ball.name} from {start} to {end} | Dir: {direction} | Dist: {Vector3.Distance(start, end)}");

        if (direction != Vector3.zero)
            ball.transform.rotation = Quaternion.LookRotation(direction);

        while (Vector3.Distance(ball.transform.position, end) > 0.5f) 
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, end, conveyorSpeed * Time.deltaTime);

            yield return null;
        }

        if (nextRapids != null)
            nextRapids.TriggerFromPrevious(ball);
        else if (cm != null)
            cm.enabled = true;
        else
            ball.transform.position = end;
        
        Debug.Log("Journey completed");

        yield return new WaitForSeconds(0.1f);

    }

    public void TriggerFromPrevious(GameObject ball)
    {
        Debug.Log($"[Rapids] Triggered from previous rapid for {ball.name}");
        StartCoroutine(MoveAlongConveyor(ball, true)); // force start snap
    }

}