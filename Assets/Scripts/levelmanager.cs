using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        Time.timeScale = 1; // Ensure the game is not paused when starting
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
            Debug.Log("Settings panel found, toggling its active state.");
            if (settingsPanel.activeSelf)
            {
                Debug.Log("Settings panel is active, closing it now.");
                settingsPanel.SetActive(false);
                Time.timeScale = 1; // Resume the game when settings panel is closed
            }
            else
            {
                Debug.Log("Settings panel is not active, opening it now.");
                settingsPanel.SetActive(true);
                Time.timeScale = 0; // Pause the game when settings panel is opened
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
        swiipeCounter swiipeCounter = Camera.main.GetComponent<swiipeCounter>();
        if (swiipeCounter != null)
        {
            swiipeCounter.adAvailable = new bool[] { true, true, true }; // Reset ad availability
        }
        Time.timeScale = 1; // Reset time scale to normal when restarting
    }
    public void level1()
    {
        // Load Level 1
        level = GameObject.Find("LevelManager").gameObject.GetComponent<LevelScript>();
        if (level != null && level.level1Unlocked)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialLevel1");
        }
    }

    public void level2()
    {
        // Load Level 1
        //Button level2Button = GameObject.Find("Level2Button").GetComponent<Button>();
        //if (level2Button != null && level2Button.interactable == true)
        //{
        //    level2Button.onClick.AddListener(() => level2());
        //}
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialL2");
            //level.level2Unlocked = true; // Unlock Level 2 after Level 1 is played
        }
    }

    public void level3()
    {
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TL3");
        }
    }

    public void level4()
    {
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TL4");
        }
    }

    public void level5()
    {
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TL5");
        }
    }
}
