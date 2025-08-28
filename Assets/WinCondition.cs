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
    int[] winCondition1 = {03, 04, 04, 05, 06, 07, 07, 15, 06, 08, 10, 10, 12, 10, 07, 09, 11, 13, 08, 11, 09, 10 };
    int[] winCondition2 = {05, 06, 05, 07, 09, 10, 09, 18, 08, 10, 12, 12, 15, 12, 09, 13, 14, 16, 10, 14, 12, 13 };
    int[] winCondition3 = {08, 08, 07, 10, 11, 12, 12, 20, 11, 13, 15, 15, 18, 15, 11, 15, 18, 20, 12, 17, 15, 16 };
    string[] strings ={ "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7",
        "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16",
        "Level_17", "Level_18", "Level_19", "Level 20", "Level_21", "Level 22"};
    public Sprite[] winconditionImages;
    int[] conditions = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
    Image Text1;
    Image Text2;
    Image Text3;
    //[SerializeField] Text swipeCounterText;
    void Awake()
    {
        blackBall = GameObject.FindGameObjectWithTag("BlackBall").GetComponent<CharacterMovement>();
        whiteBall = GameObject.FindGameObjectWithTag("WhiteBall").GetComponent<CharacterMovement>();
        //swipeCounterText = GameObject.Find("SwipeCounter").GetComponent<Text>();
        Star3 = GameObject.Find("3Star").GetComponent<Image>();
        Star2 = GameObject.Find("2Star").GetComponent<Image>();
        Star1 = GameObject.Find("1Star").GetComponent<Image>();
        Text3 = Star3.transform.Find("Text").GetComponent<Image>();
        Text2 = Star2.transform.Find("Text").GetComponent<Image>();
        Text1 = Star1.transform.Find("Text").GetComponent<Image>();

        for(int i = 0; i < strings.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == strings[i])
            {
                for(int j = 0; j< conditions.Length; j++)
                {
                    if (winCondition3[i] + 1 == conditions[j])
                    {
                        Text1.sprite = winconditionImages[j];
                    }
                    if (winCondition2[i] + 1 == conditions[j])
                    {
                        Text2.sprite = winconditionImages[j];
                    }
                    if (winCondition1[i] + 1 == conditions[j])
                    {
                        Text3.sprite = winconditionImages[j];
                    }
                }
            }
        }
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
