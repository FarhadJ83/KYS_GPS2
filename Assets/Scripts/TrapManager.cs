using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject whiteBallPrefab;
    public GameObject blackBallPrefab;

    public Transform whiteBallSpawnPoint;
    public Transform blackBallSpawnPoint;

    [SerializeField] int sc;

    public static TrapManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void RespawnBall(GameObject oldBall)
    {
        string tag = oldBall.tag;
        sc = oldBall.GetComponent<CharacterMovement>().swipeCounter;
        Destroy(oldBall); // Destroy the old ball


        if (tag == "WhiteBall")
        {
            Instantiate(whiteBallPrefab, whiteBallSpawnPoint.position, Quaternion.identity);
            //GameObject.FindGameObjectWithTag("WhiteBall").GetComponent<CharacterMovement>().assignSwipe(sc); 
        }
        else if (tag == "BlackBall")
        {
            Instantiate(blackBallPrefab, blackBallSpawnPoint.position, Quaternion.identity);
            //blackBallPrefab.GetComponent<CharacterMovement>().swipeCounter = sc;
            //oldBall.transform.position = blackBallSpawnPoint.position;
        }
    }
}
