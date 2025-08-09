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
    int[] winCondition1 = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
    int[] winCondition2 = { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
    int[] winCondition3 = {30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
    string[] strings = { "TutorialLevel1", "Level2_Completed", "TutorialL2", "Level4_Completed", 
        "Level5_Completed", "Level6_Completed", "Level7", "Level8", "TL3", "Level10", "Level11", "Level12", "Level13", "Level14","TL4", "TL5" };
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


        for(int i = 0; i < strings.Length; i++)
        {
            if (swipecount < winCondition1[i])
            {
                Star3.sprite = star;
                Star2.sprite = star;
                Star1.sprite = star;
                

                if (SceneManager.GetActiveScene().name == strings[i])
                {
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 3;
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                }
            }
            else if (swipecount < winCondition2[i])
            {
                Star3.sprite = noStar;
                Star2.sprite = star;
                Star1.sprite = star;
                if (SceneManager.GetActiveScene().name == strings[i])
                {
                    if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] < 3)
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 2;
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                }
            }
            else if (swipecount < winCondition3[i])
            {
                Star3.sprite = noStar;
                Star2.sprite = noStar;
                Star1.sprite = star;
                if (SceneManager.GetActiveScene().name == strings[i])
                {
                    if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] < 2)
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 1;
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
                        GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[i] = 0;
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
                }
            }
        }
        
    }
}
