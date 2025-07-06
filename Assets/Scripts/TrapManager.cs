using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject whiteBallPrefab;
    public GameObject blackBallPrefab;

    public Transform whiteBallSpawnPoint;
    public Transform blackBallSpawnPoint;

    public static TrapManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void RespawnBall(GameObject oldBall)
    {
        string tag = oldBall.tag;

        Destroy(oldBall);

        if (tag == "WhiteBall")
        {
            Instantiate(whiteBallPrefab, whiteBallSpawnPoint.position, Quaternion.identity);
        }
        else if (tag == "BlackBall")
        {
            Instantiate(blackBallPrefab, blackBallSpawnPoint.position, Quaternion.identity);
        }
    }
}
