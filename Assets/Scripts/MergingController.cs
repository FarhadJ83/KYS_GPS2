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
        //winScreen = GameObject.Find("WinScreen");
        //if (winScreen != null)
        //{
        //    winScreen.SetActive(false);
        //}
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
                if (mergedBall!=null)
                {
                    Instantiate(mergedBall, yinYangPosition, Quaternion.identity);
                }

                // Destroy both balls
                //if (winScreen != null)
                //{
                //    winScreen.SetActive(true);
                //}
                //StartCoroutine(WinScreen(collision));


                //winScreen.SetActive(true);
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                //Destroy(gameObject);

                for (int i = 0; i < GameObject.Find("LevelManager").GetComponent<LevelScript>().levelScenes.Length; i++)
                {
                    if (SceneManager.GetActiveScene().name == GameObject.Find("LevelManager").GetComponent<LevelScript>().levelScenes[i])
                    {
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelsUnlocked[i+1] = true;
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }   
                }
                //if (SceneManager.GetActiveScene().name == "TutorialLevel1")
                //{
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().level2Unlocked = true;
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                //}
                //else if (SceneManager.GetActiveScene().name == "TutorialL2")
                //{
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().level3Unlocked = true;
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                //}
                //else if (SceneManager.GetActiveScene().name == "TL3")
                //{
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().level4Unlocked = true;
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                //}
                //else if (SceneManager.GetActiveScene().name == "TL4")
                //{
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().level5Unlocked = true;
                //    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                //}
            }
        }

    }

    
}
