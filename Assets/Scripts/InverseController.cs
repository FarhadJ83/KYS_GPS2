using UnityEngine;

public class InverseController : MonoBehaviour
{
    public GameObject whiteEnvironment;
    public GameObject blackEnvironment;
    public Color YangColor;
    public Color YinColor;

    private bool isWhiteActive = true;

    public void InvertEnvironment()
    {
        isWhiteActive = !isWhiteActive;

        Camera.main.backgroundColor = isWhiteActive ? YangColor : YinColor;

        whiteEnvironment.SetActive(isWhiteActive);

        blackEnvironment.SetActive(!isWhiteActive);

    }
}
