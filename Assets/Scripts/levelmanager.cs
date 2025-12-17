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
    public GameObject settingsPanel;
    public GameObject winScreen;
    public GameObject creditsScreen;
    [SerializeField] GameObject mergedBall;
    int c = 0; 

    [SerializeField] private GameObject startingTransition;
    [SerializeField] private GameObject endTransition;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip WinSound;
    public AudioClip StartSound;

    Button Resume;
    Button restartW;
    Button restartP;
    Button HomeW;
    Button HomeP;
    Button nextLevel;
    [SerializeField] Button credits;
    [SerializeField] Button settings;
    [SerializeField] Button pause;
    Button inverse;
    Button back;
    Button reset;
    Action[] levels;
    string[] strings = { "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7",
        "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16",
        "Level_17", "Level_18", "Level_19", "Level 20", "Level_21", "Level 22"};
    public Sprite[] LevelStatus1;
    public Sprite[] LevelStatus2;
    public void Start()
    {
        levels = new Action[] { Level2, VLevel2, Level4, Level5, VLevel3, VLevel4, Level8, Level9, VLevel5, VLevel6, 
            Level12, Level13, VLevel7, VLevel8, Level16, Level17, Level18, VLevel9, Level20, VLevel10, Level22 };
        if (SceneManager.GetActiveScene().name != "LevelsScene" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            credits = GameObject.Find("Credit").GetComponent<Button>();
            if (credits != null)
            {
                credits.onClick.AddListener(creditPanel);
            }
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
            for(int i = 0; i < strings.Length; i++)
            {
                if (strings[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    if (i == 21)
                    {
                        nextLevel.gameObject.SetActive(false);  
                    }
                    break;
                }
            }
            if (nextLevel != null)
            {
                for (int i = 0; i < levels.Length ; i++)
                {
                    if (strings[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                    {
                        nextLevel.onClick.AddListener(() => levels[i]());
                        break;
                    }
                }
            }
            settings = GameObject.Find("Settings").GetComponent<Button>();
            if (settings != null)
            {
                settings.onClick.AddListener(settings_Panel);
            }
            HomeP = GameObject.Find("HomeP").GetComponent<Button>();
            if (HomeP != null)
            {
                HomeP.onClick.AddListener(playGame);
            }
            HomeW = GameObject.Find("HomeW").GetComponent<Button>();
            if (HomeW != null)
            {
                HomeW.onClick.AddListener(playGame);
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
            back = GameObject.Find("Back").GetComponent<Button>();
            if (back != null)
            {
                back.onClick.AddListener(settings_Panel);
            }
        }

        pausePanel = GameObject.Find("PausePanel");
        if (pausePanel != null)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    pausePanel.transform.Find("Level").GetComponent<Image>().sprite = LevelStatus2[i];
                }
            }
            pausePanel.SetActive(false);
        }
        creditsScreen = GameObject.Find("Credits");
        if (creditsScreen != null)
        {
            creditsScreen.transform.Find("Back").GetComponent<Button>().onClick.AddListener(creditPanel);
            creditsScreen.SetActive(false);
        }
        settingsPanel = GameObject.Find("SettingsPanel");
        if (settingsPanel != null)
        {
            if (SceneManager.GetActiveScene().name != "LevelsScene" && SceneManager.GetActiveScene().name != "MainMenu")
            {
                reset = GameObject.Find("Reset").GetComponent<Button>();
                reset.onClick.AddListener(ResetProgress);
            }
            settingsPanel.SetActive(false);
        }
        winScreen = GameObject.Find("WinScreen");
        if (winScreen != null)
        {
            for(int i = 0;  i < strings.Length ; i++)
            {
                if (strings[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
                {
                    winScreen.transform.Find("LevelCompleted").GetComponent<Image>().sprite = LevelStatus1[i];
                }
            }
            winScreen.SetActive(false);
        }
        

    }

    public void Update()
    {
        mergedBall = GameObject.Find("Yin Yang Ball(Clone)");
        if (mergedBall != null && c == 0)
        {
            Debug.Log("You Win2!");  
            StartCoroutine(WinScreen());
        }
    }
    public void playGame()
    {
        // Load the game scene
        Time.timeScale = 1; // Ensure the game is not paused when starting
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelsScene");
    }

    public void pause_Panel()
    {
        //GameObject pausePanel = GameObject.Find("PausePanel");
        //if (pausePanel != null)
        //{
        //    pausePanel.SetActive(false);
        //}
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
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

    public void creditPanel()
    { 
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        if (creditsScreen.activeSelf == false)
        {
            creditsScreen.SetActive(true);
        }
        else
        {
            creditsScreen.SetActive(false);
        }
    }

    public void settings_Panel()
    {
        //GameObject settingsPanel = GameObject.Find("SettingsPanel");
        //if (settingsPanel != null)
        //{
        //    settingsPanel.SetActive(false);
        //}
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        if (settingsPanel != null)
        {
            Debug.Log("Settings panel found, toggling its active state.");
            if (settingsPanel.activeSelf)
            {
                Debug.Log("Settings panel is active, closing it now.");
                settingsPanel.SetActive(false);
                //Time.timeScale = 1; // Resume the game when settings panel is closed
            }
            else
            {
                Debug.Log("Settings panel is not active, opening it now.");
                settingsPanel.SetActive(true);
                //Time.timeScale = 0; // Pause the game when settings panel is opened
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
            GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(WinSound);
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
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(StartSound);
    }

    public void MainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1; // Reset time scale to normal when returning to main menu
    }

    public void Restart()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
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
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        for (int i = 0; i < level.levelScenes.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == strings[i])
            {
                if (i < strings.Length)
                {
                    Debug.Log("Loading next level: " + strings[i+1]);
                    StartCoroutine(LoadLevelWithTransition(strings[i+1]));
                }
                else
                {
                    Debug.Log("No next level available.");
                }
            }
        }
    }

    public void VLevel1()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_1"));
    }
    public void Level2()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_2"));
    }

    public void VLevel2()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_3"));
    }

    public void Level4()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_4"));
    }

    public void Level5()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_5"));
    }
    public void VLevel3()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_6"));
    }
    public void VLevel4()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_7"));
    }

    public void Level8()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_8"));
    }
    public void Level9()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_9"));
    }
    public void VLevel5()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_10"));
    }
    public void VLevel6()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_11"));
    }
    public void Level12()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_12"));
    }
    public void Level13()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_13"));
    }
    public void VLevel7()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_14"));
    }
    public void VLevel8()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_15"));
    }
    public void Level16()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_16"));
    }
    public void Level17()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_17"));
    }
    public void Level18()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_18"));
    }
    public void VLevel9()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_19"));
    }
    public void Level20()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level 20"));
    }
    public void VLevel10()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level_21"));
    }
    public void Level22()
    {
        GameObject.Find("AudioManager").GetComponent<AudioSource>().PlayOneShot(buttonClick);
        StartCoroutine(LoadLevelWithTransition("Level 22"));
    }

    public void ResetProgress()
    {
        level.ResetProgress();
    }

}
