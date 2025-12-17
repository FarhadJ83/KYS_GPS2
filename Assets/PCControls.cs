using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PCControls : MonoBehaviour
{
    GameObject pausePanel;
    GameObject settingsPanel;
    GameObject creditsScreen;
    GameObject exitPanel;

    Button nobutton;
    Button yesbutton;

    int c;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "LevelsScene" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            // If Escape is pressed in levels and pause menu isn't active. Activate pause menu
            pausePanel = Camera.main.GetComponent<levelmanager>().pausePanel;
            settingsPanel = Camera.main.GetComponent<levelmanager>().settingsPanel;
            creditsScreen = Camera.main.GetComponent<levelmanager>().creditsScreen;
            if (pausePanel.activeSelf == false)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pause_Panel();
                }
            }
            else if (pausePanel.activeSelf == true && settingsPanel.activeSelf == true && creditsScreen.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    creditPanel();
                }
            }
            else if (pausePanel.activeSelf == true && settingsPanel.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    settingsPanel.SetActive(false);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pause_Panel();
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "LevelsScene")
        {
            c = 0;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            settingsPanel = Camera.main.GetComponent<levelmanager>().settingsPanel;
            creditsScreen = Camera.main.GetComponent<levelmanager>().creditsScreen;


            if (c == 0)
            {
                exitPanel = GameObject.Find("ExitPanel");
                nobutton = GameObject.Find("No").GetComponent<Button>();
                yesbutton = GameObject.Find("Yes").GetComponent<Button>();
                if (nobutton != null)
                {
                    nobutton.onClick.AddListener(exit_Panel);
                }
                if (yesbutton != null)
                {
                    yesbutton.onClick.AddListener(exit);
                }
                if (exitPanel != null)
                {
                    exitPanel.SetActive(false);
                    c++;
                }
            }

            if (settingsPanel.activeSelf == true && creditsScreen.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    creditPanel();
                }
            }
            else if (settingsPanel.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    settings_Panel();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exit_Panel();
                }
            }
        }
    }

    public void exit_Panel()
    {
        if (exitPanel.activeSelf == false)
        {
            exitPanel.SetActive(true);
        }
        else
        {
            exitPanel.SetActive(false);
        }
    }

    public void exit()
    {
        Application.Quit();
    }

    public void pause_Panel()
    {
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

    public void MainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1; // Reset time scale to normal when returning to main menu
    }
}
