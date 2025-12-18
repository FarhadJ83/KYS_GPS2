using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class LevelScript : MonoBehaviour
{
    private static LevelScript instance;

    [SerializeField] levelmanager levelManager;
    public int[] levelStars = new int[22];
    public Sprite NoStar;
    public Sprite Star;
    public string[] levelNames = new string[] { "1", "Level2Button", "Level3Button", "Level4Button", "Level5Button", "6", "7", "8",
        "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22"};
    public bool[] levelsUnlocked = new bool[] { true, false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false, false};
    public string[] levelScenes = { "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7",
        "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16",
        "Level_17", "Level_18", "Level_19", "Level 20", "Level_21", "Level 22"};
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
        levelScenes = new string[] { "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6", "Level_7",
        "Level_8", "Level_9", "Level_10", "Level_11", "Level_12", "Level_13", "Level_14", "Level_15", "Level_16",
        "Level_17", "Level_18", "Level_19", "Level 20", "Level_21", "Level 22"};
    }

    //private IEnumerator DeactivateActivatedButtons()
    //{
    //    for(int i = 0; i < levelScenes.Length; i++)
    //    {
    //        if (levelsUnlocked[i] == true)
    //        {
    //            LevelButtons[i].interactable = false;
    //        }
    //    }
    //    yield return new WaitForSeconds(2.0f);
    //    for (int i = 0; i < levelScenes.Length; i++)
    //    {
    //        if (levelsUnlocked[i] == true)
    //        {
    //            LevelButtons[i].interactable = true;
    //        }
    //    }
    //}

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
                //StartCoroutine(DeactivateActivatedButtons());
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
            setStars();
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
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < levelsUnlocked.Length; i++)
        {
            levelsUnlocked[i] = false;
            levelStars[i] = 0;
        }
        levelsUnlocked[0] = true; // Unlock the first level
        SaveLevelProgress();
        LoadLevelProgress();
        //LevelUnlock();
        //setStars();
    }
}
