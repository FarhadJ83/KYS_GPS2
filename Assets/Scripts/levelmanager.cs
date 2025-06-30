using UnityEngine;
using System.Collections;

public class levelmanager : MonoBehaviour
{
    public LevelScript level;
    public void playGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelsScene");
    }

    public void level1()
    {
        // Load Level 1
        level = GameObject.Find("LevelManager").gameObject.GetComponent<LevelScript>();
        if (level != null && level.level1Unlocked)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
            level.level2Unlocked = true; // Unlock Level 2 after Level 1 is played
        }
    }

    public void level2()
    {
        // Load Level 1
        level = GameObject.Find("LevelManager").gameObject.GetComponent<LevelScript>();
        if (level != null && level.level2Unlocked)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
            //level.level2Unlocked = true; // Unlock Level 2 after Level 1 is played
        }
    }
}
