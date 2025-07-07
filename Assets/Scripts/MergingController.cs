using UnityEngine;

public enum BallType { White, Black, YinYang }

public class MergingController : MonoBehaviour
{
    public BallType ballType;
    public GameObject mergedBall;
    

    private void OnCollisionEnter(Collision collision)
    {
        MergingController otherBall = collision.gameObject.GetComponent<MergingController>();

        if (otherBall != null)
        {
            bool isWhiteBlackCombo =
                (ballType == BallType.White && otherBall.ballType == BallType.Black) ||
                (ballType == BallType.Black && otherBall.ballType == BallType.White);

            if (isWhiteBlackCombo)
            {
                // Find the midpoint between the two balls
                Vector3 yinYangPosition = (transform.position + collision.transform.position) / 2f;

                // Instantiate the merged ball
                Instantiate(mergedBall, yinYangPosition, Quaternion.identity);

                // Destroy both balls
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

    }
}
