using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class swiipeCounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject swipeCountertext;
    public CharacterMovement blackBall;
    public CharacterMovement whiteBall;
    IEnumerator enumerator;
    [SerializeField] Button showRewardButton;
    [HideInInspector] public int swipecount;
    public bool [] adAvailable = {true, true, true};
    bool[] Merged;

    private void Start()
    {
        //swipeCountertext = GameObject.Find("SwipeCounter").GetComponent<Text>();
        
    }

    private void Update()
    {
        blackBall = GameObject.FindGameObjectWithTag("BlackBall").GetComponent<CharacterMovement>();
        whiteBall = GameObject.FindGameObjectWithTag("WhiteBall").GetComponent<CharacterMovement>();

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
        if (showRewardButton != null)
        {
            if (swipecount == 2 && adAvailable[0])
            {
                StartCoroutine(activateShowRewardedButton());
                adAvailable[0] = false;
            }
            if (swipecount == 5 && adAvailable[1])
            {
                StartCoroutine(activateShowRewardedButton());
                adAvailable[1] = false;
            }
            if (swipecount == 10 && adAvailable[2])
            {
                StartCoroutine(activateShowRewardedButton());
                adAvailable[2] = false;
            }
        }


    }

    IEnumerator activateShowRewardedButton()
    {

        //showRewardButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        //showRewardButton.gameObject.SetActive(false);
    }
}
