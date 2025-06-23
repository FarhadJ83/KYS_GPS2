using UnityEngine;

public class InverseController : MonoBehaviour
{
    public GameObject whiteEnvironment;
    public GameObject blackEnvironment;

    private bool isWhiteActive = true;

    public void InvertEnvironment()
    {
        isWhiteActive = !isWhiteActive;

        whiteEnvironment.SetActive(isWhiteActive);
        blackEnvironment.SetActive(!isWhiteActive);
    }
}
