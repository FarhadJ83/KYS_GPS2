using UnityEngine;
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
        }
        else if (swipecount < 20)
        {
            Star3.sprite = noStar;
            Star2.sprite = star;
            Star1.sprite = star;
        }
        else if (swipecount < 30)
        {
            Star3.sprite = noStar;
            Star2.sprite = noStar;
            Star1.sprite = star;
        }
        else
        {
            Star3.sprite = noStar;
            Star2.sprite = noStar;
            Star1.sprite = noStar;
        }
    }
}
