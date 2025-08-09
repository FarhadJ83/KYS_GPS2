using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;

public class levelmanager : MonoBehaviour
{
    public LevelScript level;
    [HideInInspector] public GameObject pausePanel;
    [HideInInspector] public GameObject settingsPanel;
    public GameObject winScreen;
    [SerializeField] GameObject mergedBall;
    int c = 0; 

    [SerializeField] private GameObject startingTransition;
    [SerializeField] private GameObject endTransition;

    Button Resume;
    Button restartW;
    Button restartP;
    Button HomeW;
    Button HomeP;
    Button nextLevel;
    Button settings;
    [SerializeField] Button pause;
    Button inverse;
    Action[] levels;
    string[] strings = { "TutorialLevel1", "Level2_Completed", "TutorialL2", "TL3", "Level5_Completed", "Level6_Completed", "Level7", "Level8",
            "Level9", "Level10", "Level11", "Level12", "Level13", "Level14", "TL4", "TL5" };
    public void Start()
    {
        levels = new Action[] { Level2, Tlevel2, Tlevel3, Tlevel4, Tlevel5 };
        if (SceneManager.GetActiveScene().name != "LevelsScene")
        {
            Resume = GameObject.Find("Resume").GetComponent<Button>();
            if (Resume != null)
            {
                Resume.onClick.AddListener(pause_Panel);
            }
            pause = GameObject.Find("Pause").GetComponent<Button>();
            if (pause != null)
            {
                pause.onClick.AddListener(pause_Panel);
            }
            nextLevel = GameObject.Find("NextLevel").GetComponent<Button>();
            if (nextLevel != null)
            {
                for (int i = 0; i < levels.Length - 1; i++)
                {
                    if (strings[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                    {
                        nextLevel.onClick.AddListener(() => levels[i]());
                        break;
                    }
                }
            }
            settings = GameObject.Find("Settings").GetComponent<Button>();
            //if (settings != null)
            //{
            //    settings.onClick.AddListener(settings_Panel);
            //}
            HomeP = GameObject.Find("HomeP").GetComponent<Button>();
            if (HomeP != null)
            {
                HomeP.onClick.AddListener(playGame);
            }
            HomeW = GameObject.Find("HomeW").GetComponent<Button>();
            if (HomeW != null)
            {
                HomeW.onClick.AddListener(MainMenu);
            }
            restartW = GameObject.Find("RestartW").GetComponent<Button>();
            if (restartW != null)
            {
                restartW.onClick.AddListener(Restart);
            }
            restartP = GameObject.Find("RestartP").GetComponent<Button>();
            if (restartP != null)
            {
                restartP.onClick.AddListener(Restart);
            }
        }

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
        winScreen = GameObject.Find("WinScreen");
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }

    }

    public void Update()
    {
        mergedBall = GameObject.Find("Yin Yang Ball(Clone)");
        if (mergedBall != null && c == 0)
        {
            StartCoroutine(WinScreen());
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

        if (pausePanel.activeSelf == false) 
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

    private IEnumerator WinScreen()
    {
        if (winScreen != null)
        {
            c++;
            yield return new WaitForSeconds(2f);
            startingTransition.SetActive(true);
            yield return new WaitForSeconds(2f);
            startingTransition.SetActive(false);
            endTransition.SetActive(true);
            
            //yield return new WaitForSeconds(4f); // Show the win screen for 2 seconds

            winScreen.SetActive(true);
        }
    }

    private IEnumerator LoadLevelWithTransition(string levelName)
    {
        if (startingTransition != null)
        {
            startingTransition.SetActive(true);
        }

        // Wait for the transition animation (adjust duration as needed)
        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
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

    public void next_Level()
    {
        for (int i = 0; i < level.levelScenes.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == level.levelScenes[i])
            {
                if (i + 1 < level.levelScenes.Length)
                {
                    StartCoroutine(LoadLevelWithTransition(level.levelScenes[i + 1]));
                }
                else
                {
                    Debug.Log("No next level available.");
                }
            }
        }
    }
    public void Tlevel1()
    {
        // Load Level 1
        {
            StartCoroutine(LoadLevelWithTransition("TutorialLevel1"));
        }
    }

    public void Level2()
    {   Debug.Log("Level 2 button clicked");
        {
            StartCoroutine(LoadLevelWithTransition("Level2_Completed"));
        }
    }

    public void Tlevel2()
    {
        // Load Level 1
        //Button level2Button = GameObject.Find("Level2Button").GetComponent<Button>();
        //if (level2Button != null && level2Button.interactable == true)
        //{
        //    level2Button.onClick.AddListener(() => level2());
        //}
        {
            StartCoroutine(LoadLevelWithTransition("TutorialL2"));
            //level.level2Unlocked = true; // Unlock Level 2 after Level 1 is played
        }
    }

    public void Level5()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level5_Completed"));
        }
    }

    public void Level6()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level6_Completed"));
        }
    }

    public void Level7()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level7"));
        }
    }

    public void Level8()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level8"));
        }
    }

    public void Level9()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level9"));
        }
    }

    public void Level10()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level10"));
        }
    }

    public void Level11()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level11"));
        }
    }

    public void Level12()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level12"));
        }
    }

    public void Level13()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level13"));
        }
    }

    public void Level14()
    {
        {
            StartCoroutine(LoadLevelWithTransition("Level14"));
        }
    }

    public void Tlevel3()
    {
        {
            StartCoroutine(LoadLevelWithTransition("TL3"));
        }
    }

    public void Tlevel4()
    {
        {
            StartCoroutine(LoadLevelWithTransition("TL4"));
        }
    }

    public void Tlevel5()
    {
        {
            StartCoroutine(LoadLevelWithTransition("TL5"));
        }
    }
}
