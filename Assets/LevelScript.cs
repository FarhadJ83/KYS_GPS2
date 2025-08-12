using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelScript : MonoBehaviour
{
    private static LevelScript instance;

    [SerializeField] levelmanager levelManager;
    public int[] levelStars = new int[10];
    public Sprite NoStar;
    public Sprite Star;
    public string[] levelNames = new string[] { "1", "Level2Button", "Level3Button", "Level4Button", "Level5Button", "6", "7", "8", "9", "10"};
    public bool[] levelsUnlocked = new bool[] { true, false, false, false, false, false, false, false, false, false};
    public string[] levelScenes = { "VertLvl1(Level_1)", "VertLvl2_(Level_3)", "VertLvl3_(Level_6)", "VertLvl4_(Level_7)", "VertLvl5_(Level10)", "VertLvl6(Level_11)",
        "VertLvl7(Level_14)", "VertLvl8(Level_15)", "VertLvl9_(Level19)", "VertLvl10_(Level21)"};
    public Button[] LevelButtons;
    public Sprite[] levelImage;
    bool nextPage = false;
    int max, min;

    private void Awake()
    {   
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLevelProgress();
            SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded();
        }
        else
        {
            // An instance already exist, so delete this one
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelScenes = new string[] { "VertLvl1(Level_1)", "VertLvl2_(Level_3)", "VertLvl3_(Level_6)", "VertLvl4_(Level_7)", "VertLvl5_(Level10)", "VertLvl6(Level_11)",
            "VertLvl7(Level_14)", "VertLvl8(Level_15)", "VertLvl9_(Level19)", "VertLvl10_(Level21)"};
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "LevelsScene")
        {
            LevelUnlock();
            setStars();
            LoadLevelProgress();
        }

        if (SceneManager.GetActiveScene().name == "LevelsScene")
            for (int i = 0; i < LevelButtons.Length; i++)
            {
                LevelButtons[i] = GameObject.Find(levelNames[i]).GetComponent<Button>();
                if (i > 7 && !nextPage)
                {
                    LevelButtons[i].gameObject.SetActive(false);
                }
                else if (i < 2 && nextPage)
                {
                    LevelButtons[i].gameObject.SetActive(false);
                }
            }
    }

    public void nxtPage()
    {
        if (nextPage)
        { 
            nextPage = false;
            OnSceneLoaded();
        }
        else if (!nextPage)
        {
            nextPage = true;
            OnSceneLoaded();
        }
    }

    public void Update()
    {
        levelManager = Camera.main.GetComponent<levelmanager>();

        if (SceneManager.GetActiveScene().name == "LevelsScene")
        {
            LevelUnlock();
            //setStars();
            LoadLevelProgress();
        }


        if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            ResetProgress();
        }
    }

    public void LevelUnlock()
    {
        for (int i = 0; i < levelsUnlocked.Length; i++)
        {
            if (LevelButtons[i] != null)
            {
                if (levelsUnlocked[i])
                {
                    if (LevelButtons[i] != null)
                    {
                        LevelButtons[i].image.sprite = levelImage[i];
                        LevelButtons[i].interactable = true;
                    }
                }
            }
        }
    }

    //Save the level progress and Stars

    public void SaveLevelProgress()
    {
        for (int i = 0; i < levelStars.Length; i++)
        {
            PlayerPrefs.SetInt("Level" + (i + 1) + "Stars", levelStars[i]);
            PlayerPrefs.SetInt("Level" + (i + 1) + "Unlocked", levelsUnlocked[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    //Load the level progress and Stars

    public void LoadLevelProgress()
    {
        for (int i = 0; i < levelStars.Length; i++)
        {
            levelStars[i] = PlayerPrefs.GetInt("Level" + (i + 1) + "Stars", 0);
            levelsUnlocked[i] = PlayerPrefs.GetInt("Level" + (i + 1) + "Unlocked", 0) == 1;
        }
    }

    public void setStars()
    {
        for (int i = 0; i < levelStars.Length; i++)
        {
            GameObject Star1 = GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("1Star").gameObject;
            GameObject Star2 = GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("2Star").gameObject;
            GameObject Star3 = GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("3Star").gameObject;

            if (levelStars[i] == 3)
            {
                Debug.Log("Setting stars for level 1");
                Star1.GetComponent<Image>().sprite = Star;
                Star2.GetComponent<Image>().sprite = Star;
                Star3.GetComponent<Image>().sprite = Star;
            }
            else if (levelStars[i] == 2)
            {
                Star1.GetComponent<Image>().sprite = Star;
                Star2.GetComponent<Image>().sprite = Star;
                Star3.GetComponent<Image>().sprite = NoStar;
            }
            else if (levelStars[i] == 1)
            {
                Star1.GetComponent<Image>().sprite = Star;
                Star2.GetComponent<Image>().sprite = NoStar;
                Star3.GetComponent<Image>().sprite = NoStar;
            }
            else if (levelStars[i] == 0)
            {
                Star1.GetComponent<Image>().sprite = NoStar;
                Star2.GetComponent<Image>().sprite = NoStar;
                Star3.GetComponent<Image>().sprite = NoStar;
            }   
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Deletes all keys and values from PlayerPrefs
        Debug.LogWarning("PLAYER PROGRESS RESET!"); // Use a warning to make it stand out
    }
}
