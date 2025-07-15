using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class swiipeCounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject swipeCountertext;
    [SerializeField] CharacterMovement blackBall;
    [SerializeField] CharacterMovement whiteBall;
    int swipecount;

    private void Start()
    {
        //swipeCountertext = GameObject.Find("SwipeCounter").GetComponent<Text>();
        blackBall = GameObject.Find("Black Ball").GetComponent<CharacterMovement>();
        whiteBall = GameObject.Find("White Ball").GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        swipeCountertext = GameObject.Find("SwipeCounter");
        if (blackBall.swipeCounter >= whiteBall.swipeCounter)
        {
            swipecount = blackBall.swipeCounter;
            whiteBall.swipeCounter = blackBall.swipeCounter;
        }
        else
        {
            swipecount = whiteBall.swipeCounter;
            blackBall.swipeCounter = whiteBall.swipeCounter;
        }
        
        swipeCountertext.GetComponent<TextMeshProUGUI>().text = "Swipe Count: " + swipecount.ToString();
    }
}
