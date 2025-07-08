using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public enum BallType { White, Black, YinYang }

public class MergingController : MonoBehaviour
{
    public BallType ballType;
    public GameObject mergedBall;
    GameObject winScreen; 



    private void Start()
    {
        // Optionally, you can initialize the win screen here if needed
        winScreen = GameObject.Find("WinScreen");
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }
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
                //StartCoroutine(WinScreen());
                // Destroy both balls
                if (winScreen != null)
                {
                    winScreen.SetActive(true);
                }

                

                //winScreen.SetActive(true);
                //Destroy(collision.gameObject);
                //Destroy(gameObject);
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);

                if (SceneManager.GetActiveScene().name == "TutorialLevel1")
                {
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().level2Unlocked = true;
                }
                else if (SceneManager.GetActiveScene().name == "TutorialL2")
                {
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().level3Unlocked = true;
                }
                else if (SceneManager.GetActiveScene().name == "TL3")
                {
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().level4Unlocked = true;
                }
                else if (SceneManager.GetActiveScene().name == "TL4")
                {
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().level5Unlocked = true;
                }
            }
        }

    }
}
