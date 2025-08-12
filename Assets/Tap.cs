using UnityEngine;
using UnityEngine.SceneManagement;

public class Tap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        // We use the provided TouchController for tap input
        TouchController.onTap += HandleTap;
    }

    private void OnDisable()
    {
        TouchController.onTap -= HandleTap;
    }

    private void HandleTap(object sender, TouchGesture e)
    {
        // On tap, load the Menu Scene
        SceneManager.LoadScene("LevelsScene");
    }

}
