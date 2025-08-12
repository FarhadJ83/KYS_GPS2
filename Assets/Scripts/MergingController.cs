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
    public bool isMerged = false;


    private void Start()
    {
        
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
                    isMerged = true;
                }

                // Set SwipeCounter Component to inactive
                Camera.main.GetComponent<swiipeCounter>().enabled = false;
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                //Destroy(gameObject);

                for (int i = 0; i < GameObject.Find("LevelManager").GetComponent<LevelScript>().levelScenes.Length - 1; i++)
                {
                    if (SceneManager.GetActiveScene().name == GameObject.Find("LevelManager").GetComponent<LevelScript>().levelScenes[i])
                    {
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelsUnlocked[i+1] = true;
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }   
                }
                
            }
        }

    }

    
}
