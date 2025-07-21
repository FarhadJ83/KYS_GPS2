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
    public bool level1Unlocked = true;
    public bool level2Unlocked = false;
    public bool level3Unlocked = false;
    public bool level4Unlocked = false;
    public bool level5Unlocked = false;
    [SerializeField] Button Level2Button;
    [SerializeField] Button Level3Button;
    [SerializeField] Button Level4Button;
    [SerializeField] Button Level5Button;
    public Sprite level3Img;
    public Sprite level2Img;
    public Sprite level4Img;
    public Sprite level5Img;
    int levelnumber = 1;
    public int[] levelStars = new int[5];
    public Sprite NoStar;
    public Sprite Star;
    string[] levelNames = new string[5] { "1", "Level2Button", "Level3Button", "Level4Button", "Level5Button"};


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
            Level2Unlock();
            Level3Unlock();
            Level4Unlock();
            Level5Unlock();
            setStars();
        }

        if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            ResetProgress();
        }
    }

    public void Level2Unlock()
    {
        Level2Button = GameObject.Find("Level2Button").GetComponent<Button>();
        if (Level2Button != null)
        {
            if (level2Unlocked)
            {
                Level2Button.image.sprite = level2Img;
                Level2Button.interactable = true;
                //Level2Button.onClick.AddListener(() => levelManager.level2());
            }
        }
    }

    public void Level3Unlock()
    {
        Level3Button = GameObject.Find("Level3Button").GetComponent<Button>();
        if (Level3Button != null)
        {
            if (level3Unlocked)
            {
                Level3Button.image.sprite = level3Img;
                Level3Button.interactable = true;
                //Level3Button.onClick.AddListener(() => levelManager.level3());
            }
        }
    }

    public void Level4Unlock()
    {
        Level4Button = GameObject.Find("Level4Button").GetComponent<Button>();
        if (Level4Button != null)
        {
            if (level4Unlocked)
            {
                Level4Button.image.sprite = level4Img;
                Level4Button.interactable = true;
                //Level4Button.onClick.AddListener(() => levelManager.level4());
            }
        }
    }

    public void Level5Unlock()
    {
        Level5Button = GameObject.Find("Level5Button").GetComponent<Button>();
        if (Level5Button != null)
        {
            if (level5Unlocked)
            {
                Level5Button.image.sprite = level5Img;
                Level5Button.interactable = true;
                //Level5Button.onClick.AddListener(() => levelManager.level5());
            }
        }
    }

    //Save the level progress
    public void SaveLevelProgress()
    {
        PlayerPrefs.SetInt("Level1Unlocked", level1Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Level2Unlocked", level2Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Level3Unlocked", level3Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Level4Unlocked", level4Unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Level5Unlocked", level5Unlocked ? 1 : 0);

        //Save the level stars

        for (int i = 0; i < levelStars.Length; i++)
        {
            PlayerPrefs.SetInt("Level" + (i + 1) + "Stars", levelStars[i]);
        }

        PlayerPrefs.Save();

    }

    public void LoadLevelProgress()
    {
        // Load the saved value for each level.
        // The second argument (0) is the default value if the key is not found.
        // We check if the value is 1 to convert it back to a boolean.
        level1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 1) == 1; // Default to unlocked
        level2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
        level3Unlocked = PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;
        level4Unlocked = PlayerPrefs.GetInt("Level4Unlocked", 0) == 1;
        level5Unlocked = PlayerPrefs.GetInt("Level5Unlocked", 0) == 1;

        // Load the level stars

        for (int i = 0; i < levelStars.Length; i++)
        {
            levelStars[i] = PlayerPrefs.GetInt("Level" + (i + 1) + "Stars", 0); // Default to 0 stars
        }

        Debug.Log("Level progress loaded!");
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
