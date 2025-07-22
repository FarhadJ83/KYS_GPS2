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
    //[SerializeField] Text swipeCounterText;
    void Awake()
    {
        blackBall = GameObject.Find("Black Ball").GetComponent<CharacterMovement>();
        whiteBall = GameObject.Find("White Ball").GetComponent<CharacterMovement>();
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



        if (swipecount < 10)
        {
            Star3.sprite = star;
            Star2.sprite = star;
            Star1.sprite = star;

            if (SceneManager.GetActiveScene().name == "TutorialLevel1")
            {
                GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] = 3;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();

            }
            else if (SceneManager.GetActiveScene().name == "TutorialL2")
            {
                GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] = 3;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL3")
            {
                GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] = 3;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL4")
            {
                GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] = 3;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL5")
            {
                GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] = 3;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
        }
        else if (swipecount < 20)
        {
            Star3.sprite = noStar;
            Star2.sprite = star;
            Star1.sprite = star;
            if (SceneManager.GetActiveScene().name == "TutorialLevel1")
            {
                if(GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] < 3)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] = 2;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TutorialL2")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] < 3)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] = 2;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL3")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] < 3)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] = 2;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL4")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] < 3)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] = 2;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL5")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] < 3)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] = 2;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
        }
        else if (swipecount < 30)
        {
            Star3.sprite = noStar;
            Star2.sprite = noStar;
            Star1.sprite = star;

            if (SceneManager.GetActiveScene().name == "TutorialLevel1")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] < 2)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] = 1;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TutorialL2")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] < 2)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] = 1;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL3")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] < 2)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] = 1;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL4")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] < 2)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] = 1;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL5")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] < 2)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] = 1;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
        }
        else
        {
            Star3.sprite = noStar;
            Star2.sprite = noStar;
            Star1.sprite = noStar;

            if (SceneManager.GetActiveScene().name == "TutorialLevel1")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] < 1)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[0] = 0;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TutorialL2")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] < 1)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[1] = 0;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL3")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] < 1)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[2] = 0;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL4")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] < 1)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[3] = 0;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
            else if (SceneManager.GetActiveScene().name == "TL5")
            {
                if (GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] < 1)
                    GameObject.Find("LevelManager").GetComponent<LevelScript>().levelStars[4] = 0;
                GameObject.Find("LevelManager").GetComponent<LevelScript>().SaveLevelProgress();
            }
        }
    }
}
