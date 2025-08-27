using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] CharacterMovement blackBall;
    [SerializeField] CharacterMovement whiteBall;
    int swipecount;
    Image Star3;
    Image Star2;
    Image Star1;
    public Sprite star;
    public Sprite noStar;
    int[] winCondition1 = {3, 4, 7, 7, 8, 10, 10, 7, 8, 9, 3, 4, 7, 7, 8, 10, 10, 7, 8, 9, 0, 0 };
    int[] winCondition2 = {5, 5, 9, 9, 10, 12, 12, 9, 10, 13, 3, 4, 7, 7, 8, 10, 10, 7, 8, 9, 0, 0 };
    int[] winCondition3 = {8, 7, 10, 12, 13, 15, 15, 11, 12, 16, 3, 4, 7, 7, 8, 10, 10, 7, 8, 9, 0, 0 };
    string[] strings ={ "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7",
        "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16",
        "Level_17", "Level_18", "Level19", "Level_20", "Level_21", "Level_22"};
    //[SerializeField] Text swipeCounterText;
    void Awake()
    {
        blackBall = GameObject.FindGameObjectWithTag("BlackBall").GetComponent<CharacterMovement>();
        whiteBall = GameObject.FindGameObjectWithTag("WhiteBall").GetComponent<CharacterMovement>();
        //swipeCounterText = GameObject.Find("SwipeCounter").GetComponent<Text>();
        Star3 = GameObject.Find("3Star").GetComponent<Image>();
        Star2 = GameObject.Find("2Star").GetComponent<Image>();
        Star1 = GameObject.Find("1Star").GetComponent<Image>();
    }

    private void Update()
    {
        if(blackBall.swipeCounter >= whiteBall.swipeCounter)
        {
            swipecount = blackBall.swipeCounter;
        }
        else
        {
            swipecount = whiteBall.swipeCounter;
        }

        GameObject levelManager = GameObject.Find("LevelManager");

        for (int i = 0; i < strings.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == strings[i])
            {
                if (swipecount <= winCondition1[i])
                {
                    Debug.Log("Swipe count: " + swipecount + " | Win condition 1: " + winCondition1[i] + " | Level: " + strings[i]);
                    Star3.sprite = star;
                    Star2.sprite = star;
                    Star1.sprite = star;
                    
                    {
                        Debug.Log(i);
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 3;
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }
                }

                else if (swipecount <= winCondition2[i])
                {
                    Debug.Log("Swipe count: " + swipecount + " | Win condition 2: " + winCondition2[i] + " | Level: " + strings[i]);
                    Star3.sprite = noStar;
                    Star2.sprite = star;
                    Star1.sprite = star;
                    if (SceneManager.GetActiveScene().name == strings[i])
                    {
                        if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] < 3)
                        {
                            GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 2;
                        }
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }
                }
                else if (swipecount <= winCondition3[i])
                {
                    Star3.sprite = noStar;
                    Star2.sprite = noStar;
                    Star1.sprite = star;
                    if (SceneManager.GetActiveScene().name == strings[i])
                    {
                        if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] < 2)
                        {
                            GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 1;
                        }
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }
                }
                else
                {
                    Star3.sprite = noStar;
                    Star2.sprite = noStar;
                    Star1.sprite = noStar;
                    if (SceneManager.GetActiveScene().name == strings[i])
                    {
                        if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] < 1)
                        {
                            GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 0;
                        }
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                    }
                }
            }
        }
        
    }
}
