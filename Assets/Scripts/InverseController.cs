using UnityEngine;

public class InverseController : MonoBehaviour
{
    [HideInInspector] GameObject whiteEnvironment;
    [HideInInspector] GameObject blackEnvironment;
    public Color YangColor;
    public Color YinColor;

    private bool isWhiteActive = true;

    public void Awake()
    {
        whiteEnvironment = GameObject.Find("Yang World");
        blackEnvironment = GameObject.Find("Yin World");
        blackEnvironment.SetActive(false);
        Camera.main.backgroundColor = YangColor; 
    }
    public void InvertEnvironment()
    {
        isWhiteActive = !isWhiteActive;

        Camera.main.backgroundColor = isWhiteActive ? YangColor : YinColor;

        whiteEnvironment.SetActive(isWhiteActive);

        blackEnvironment.SetActive(!isWhiteActive);

    }
}
//8D8D8D