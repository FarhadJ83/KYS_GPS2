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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhiteBall") || other.CompareTag("BlackBall"))
        {
            // Disable character control
            CharacterMovement cm = other.GetComponent<CharacterMovement>();
            if (cm != null)
            {
                cm.enabled = false;
            }

            // Start conveyor movement
            StartCoroutine(MoveAlongConveyor(other.gameObject));
        }
    }

    private IEnumerator MoveAlongConveyor(GameObject ball)
    {
        float t = 0f;
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(ball.transform.position, end) > 0.05f)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, end, conveyorSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to final position
        ball.transform.position = end;

        // Re-enable character control
        CharacterMovement cm = ball.GetComponent<CharacterMovement>();
        if (cm != null)
        {
            cm.enabled = true;
        }
    }
}
