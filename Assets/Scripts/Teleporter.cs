using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("Where the new ball should appear.")]
    public Transform destinationTeleporter;

    [Tooltip("The White Ball prefab.")]
    public GameObject whiteBallPrefab;

    [Tooltip("The Black Ball prefab.")]
    public GameObject blackBallPrefab;

    public AudioClip portalsound;

    private void OnCollisionEnter(Collision collision)
    {
        // Only respond to white or black balls
        if (collision.gameObject.CompareTag("WhiteBall") || collision.gameObject.CompareTag("BlackBall"))
        {
            GameObject originalBall = collision.gameObject;

            // Get last movement direction
            Vector3 moveDir = Vector3.zero;
            CharacterMovement originalMove = originalBall.GetComponent<CharacterMovement>();
            if (originalMove != null)
                moveDir = originalMove.lastMoveDir;

            // Detect ball type from MergingController
            BallType type = BallType.White;
            MergingController mc = originalBall.GetComponent<MergingController>();
            if (mc != null)
                type = mc.ballType;

            // Destroy the original ball
            Destroy(originalBall);

            // Instantiate the correct prefab
            GameObject prefabToSpawn = (type == BallType.White) ? whiteBallPrefab : blackBallPrefab;

            if (destinationTeleporter != null && prefabToSpawn != null)
            {
                // Spawn new ball at destination
                GameObject newBall = Instantiate(prefabToSpawn, destinationTeleporter.position, Quaternion.identity);
                newBall.transform.forward = moveDir;

                // Restore ball type in new MergingController
                MergingController mcNew = newBall.GetComponent<MergingController>();
                if (mcNew != null)
                    mcNew.ballType = type;

                // Move the new ball using CharacterMovement logic
                CharacterMovement cm = newBall.GetComponent<CharacterMovement>();
                if (cm != null && moveDir != Vector3.zero)
                {
                    cm.lastMoveDir = moveDir;

                    Vector3 origin = newBall.transform.position;

                    // Use the same gate/wall/rapid logic as swipe movement
                    Vector3 targetPos = cm.GetTargetPosition(origin, moveDir);

                    // Debug line
                    Debug.DrawRay(origin, moveDir * 5f, Color.magenta, 1.0f);
                    Debug.Log($"[Teleport] Moving new ball toward: {targetPos}");

                    // Move the ball
                    cm.StartCoroutine(cm.SendTo(targetPos));
                }
                else
                {
                    Debug.LogWarning("[Teleport] CharacterMovement missing or moveDir is zero.");
                }
                GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(portalsound);

            }
        }
    }
}
