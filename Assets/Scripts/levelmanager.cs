using UnityEngine;
using System.Collections;

public class levelmanager : MonoBehaviour
{
    public LevelScript level;
    [HideInInspector] public GameObject pausePanel;
    [HideInInspector] public GameObject settingsPanel;
    public void Start()
    {
        pausePanel = GameObject.Find("PausePanel");
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        settingsPanel = GameObject.Find("SettingsPanel");
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    public void playGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelsScene");
    }

    public void pause_Panel()
    {
        //GameObject pausePanel = GameObject.Find("PausePanel");
        //if (pausePanel != null)
        //{
        //    pausePanel.SetActive(false);
        //}

        if (Time.timeScale == 1)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0; // Pause the game
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1; // Pause the game
        }
    }

    public void settings_Panel()
    {
        //GameObject settingsPanel = GameObject.Find("SettingsPanel");
        //if (settingsPanel != null)
        //{
        //    settingsPanel.SetActive(false);
        //}
        if (settingsPanel != null)
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
            }
            else
            {
                settingsPanel.SetActive(true);
            }
        }
    }

    public void MainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1; // Reset time scale to normal when returning to main menu
    }

    public void Restart()
    {
        // Restart the current level
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1; // Reset time scale to normal when restarting
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
