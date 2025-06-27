using UnityEngine;

public enum BallType { White, Black, YinYang }

public class MergingController : MonoBehaviour
{
    public BallType ballType;
    public GameObject mergedBall;

    private void OnTriggerEnter(Collider other)
    {
        MergingController otherBall = other.GetComponent<MergingController>();

        if (otherBall != null )
        {
            if ((ballType == BallType.White && otherBall.ballType == BallType.Black)  || (ballType == BallType.Black && otherBall.ballType == BallType.White))
            {
                Vector3 yinYangPosition = (transform.position + other.transform.position) / 2f;

                Instantiate(mergedBall, yinYangPosition, Quaternion.identity);

                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
