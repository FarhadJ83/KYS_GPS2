using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelScript : MonoBehaviour
{
    private LevelScript instance;

    [SerializeField] levelmanager levelManager;
    public int[] levelStars = new int[6];
    public Sprite NoStar;
    public Sprite Star;
    string[] levelNames = new string[6] { "1", "Level2Button", "Level3Button", "Level4Button", "Level5Button", "6"};
    public bool[] levelsUnlocked = new bool[6] { true, false, false, false, false, false};
    public string[] levelScenes = new string[6] { "TutorialLevel1", "Level2_Completed", "TutorialL2", "TL3", "TL4", "TL5" };
    public Button[] LevelButtons;
    public Sprite[] levelImage;


    private void Awake()
    {   
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLevelProgress();
        }
        else
        {
            // An instance already exist, so delete this one
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        levelManager = Camera.main.GetComponent<levelmanager>();
        if (SceneManager.GetActiveScene().name == "LevelsScene")
        {
            for (int i = 0; i < LevelButtons.Length; i++)
            {
                LevelButtons[i] = GameObject.Find(levelNames[i]).GetComponent<Button>();
            }
            LevelUnlock();
            setStars();
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
            if (levelStars[i] == 3)
            {
                Debug.Log("Setting stars for level 1");
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("1Star").GetComponent<Image>().sprite = Star;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("2Star").GetComponent<Image>().sprite = Star;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("3Star").GetComponent<Image>().sprite = Star;
            }
            else if (levelStars[i] == 2)
            {
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("1Star").GetComponent<Image>().sprite = Star;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("2Star").GetComponent<Image>().sprite = Star;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("3Star").GetComponent<Image>().sprite = NoStar;
            }
            else if (levelStars[i] == 1)
            {
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("1Star").GetComponent<Image>().sprite = Star;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("2Star").GetComponent<Image>().sprite = NoStar;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("3Star").GetComponent<Image>().sprite = NoStar;
            }
            else
            {
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("1Star").GetComponent<Image>().sprite = NoStar;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("2Star").GetComponent<Image>().sprite = NoStar;
                GameObject.Find(levelNames[i]).transform.Find("LevelStars").transform.Find("3Star").GetComponent<Image>().sprite = NoStar;
            }
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Deletes all keys and values from PlayerPrefs
        Debug.LogWarning("PLAYER PROGRESS RESET!"); // Use a warning to make it stand out
    }
}
