using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Where the new ball should appear.")]
    public Transform destinationTeleporter;

    [Tooltip("The ball prefab to instantiate.")]
    public GameObject ballPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            GameObject originalBall = collision.gameObject;

            // Get last movement direction from original
            Vector3 moveDir = Vector3.zero;
            CharacterMovement originalMove = originalBall.GetComponent<CharacterMovement>();
            if (originalMove != null)
            {
                moveDir = originalMove.lastMoveDir;
            }

            // Store ball type if needed
            BallType type = BallType.White;
            MergingController mc = originalBall.GetComponent<MergingController>();
            if (mc != null)
            {
                type = mc.ballType;
            }

            // Destroy the original ball
            Destroy(originalBall);

            // Instantiate new ball at destination
            if (destinationTeleporter != null && ballPrefab != null)
            {
                // Spawn new ball at destination
                GameObject newBall = Instantiate(ballPrefab, destinationTeleporter.position, Quaternion.identity);

                // Set ball type (if it has merging behavior)
                MergingController mcNew = newBall.GetComponent<MergingController>();
                if (mcNew != null)
                {
                    mcNew.ballType = type;
                }

                // Move in the last direction
                CharacterMovement cm = newBall.GetComponent<CharacterMovement>();
                if (cm != null && moveDir != Vector3.zero)
                {
                    cm.lastMoveDir = moveDir;

                    Vector3 origin = newBall.transform.position;
                    Vector3 targetPos;

                    // Raycast to check for wall in direction
                    if (Physics.Raycast(origin, moveDir, out RaycastHit wallHit, Mathf.Infinity, cm.wallLayer))
                    {
                        targetPos = wallHit.point - moveDir * cm.raycastPadding;
                    }
                    else
                    {
                        targetPos = origin + moveDir * 10f; // fallback
                    }

                    // Optional debug to verify
                    Debug.DrawRay(origin, moveDir * 5f, Color.cyan, 1.0f);
                    Debug.Log($"[Teleport] Moving new ball toward: {targetPos}");

                    // Start the movement coroutine
                    cm.StartCoroutine(cm.SendTo(targetPos));
                }
                else
                {
                    Debug.LogWarning("[Teleport] CharacterMovement missing or moveDir is zero.");
                }
            }
        }
    }
}
