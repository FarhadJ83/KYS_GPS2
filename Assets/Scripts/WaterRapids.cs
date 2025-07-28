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
            // Start conveyor movement
            StartCoroutine(MoveAlongConveyor(other.gameObject));
        }
    }

    private IEnumerator MoveAlongConveyor(GameObject ball)
    {
        // Disable character control
        CharacterMovement cm = ball.GetComponent<CharacterMovement>();
        if (cm != null)
        {
            cm.enabled = false;
        }

        yield return new WaitForSeconds(0.2f);

        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        // Optional: Rotate ball to face movement direction
        Vector3 direction = (end - start).normalized;
        if (direction != Vector3.zero)
        {
            ball.transform.rotation = Quaternion.LookRotation(direction);
        }

        while (Vector3.Distance(ball.transform.position, end) > 0.05f)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, end, conveyorSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to exact end position
        ball.transform.position = end;

        yield return new WaitForSeconds(0.1f);

        if (nextRapids != null)
        {
            // Pass to next connected rapid
            nextRapids.TriggerFromPrevious(ball);
        }
        else
        {
            // Re-enable control if no next rapid
            if (cm != null)
                cm.enabled = true;
        }
    }

    // Call this when coming from a previous rapid (to avoid re-triggering on collision)
    public void TriggerFromPrevious(GameObject ball)
    {
        StartCoroutine(MoveAlongConveyor(ball));
    }
}
