using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))] // Ensure there's a trigger
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
        // Check tags and also ensure we don't re-trigger for a ball already on a rapids course.
        if (other.CompareTag("WhiteBall") || other.CompareTag("BlackBall"))
        {
            // A simple way to prevent re-triggering is to check if the movement script is enabled.
            CharacterMovement cm = other.GetComponent<CharacterMovement>();
            if (cm != null && cm.enabled)
            {
                Debug.Log($"[Rapids] Triggered by {other.name}");
                StartCoroutine(MoveAlongConveyor(other.gameObject, false));
            }
        }
    }

    private IEnumerator MoveAlongConveyor(GameObject ball, bool forceStartAtPoint)
    {
        if (ball == null) yield break; // Safety check

        // --- Component Caching & State Management ---
        CharacterMovement cm = ball.GetComponent<CharacterMovement>();
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (cm != null)
            cm.enabled = false;

        bool originalIsKinematic = false;
        bool originalUseGravity = true;

        if (rb != null)
        {
            // Store original Rigidbody state
            originalIsKinematic = rb.isKinematic;
            originalUseGravity = rb.useGravity;

            // Take control from the physics engine
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero; // Stop any existing movement
        }

        // Optional delay to let the ball settle
        yield return new WaitForSeconds(0.1f);

        // --- Robust Time-Based Movement ---
        Vector3 startPos = forceStartAtPoint ? startPoint.position : ball.transform.position;
        Vector3 endPos = endPoint.position;

        // Snap ball to the starting position if forced
        if (forceStartAtPoint)
        {
            ball.transform.position = startPos;
        }

        float totalDistance = Vector3.Distance(startPos, endPos);
        if (totalDistance <= 0f) // Avoid division by zero if points are the same
        {
            Debug.LogWarning("[Rapids] Start and End points are the same. Finishing early.");
        }
        else
        {
            Vector3 direction = (endPos - startPos).normalized;
            if (direction != Vector3.zero)
            {
                ball.transform.rotation = Quaternion.LookRotation(direction);
            }

            float duration = totalDistance / conveyorSpeed;
            float elapsedTime = 0f;

            Debug.Log($"[Rapids] Moving {ball.name} to {endPos}. Duration: {duration:F2}s");

            while (elapsedTime < duration)
            {
                if (ball == null) yield break; // Ball might be destroyed mid-coroutine

                // Use Lerp for smooth, predictable movement
                Vector3 newPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

                // Use Rigidbody.MovePosition for physics-friendly movement
                if (rb != null)
                {
                    rb.MovePosition(newPosition);
                }
                else // Fallback for objects without a Rigidbody
                {
                    ball.transform.position = newPosition;
                }

                elapsedTime += Time.deltaTime;
                yield return null; // Wait for the next frame
            }
        }

        // --- Finalization and Chaining ---

        // Ensure the ball is exactly at the end point
        ball.transform.position = endPos;
        if (rb != null) rb.MovePosition(endPos);

        yield return new WaitForSeconds(0.1f); // Small delay before next action

        if (nextRapids != null)
        {
            // Pass control to the next rapids
            nextRapids.TriggerFromPrevious(ball);
        }
        else
        {
            // Restore control to the player/physics
            if (rb != null)
            {
                rb.isKinematic = originalIsKinematic;
                rb.useGravity = originalUseGravity;
            }
            if (cm != null)
            {
                cm.enabled = true;
            }
        }
    }

    public void TriggerFromPrevious(GameObject ball)
    {
        Debug.Log($"[Rapids] Triggered from previous rapid for {ball.name}");
        StartCoroutine(MoveAlongConveyor(ball, true)); // Force start snap for seamless chaining
    }
}