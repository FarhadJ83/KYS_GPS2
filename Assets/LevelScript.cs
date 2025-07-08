using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    private LevelScript instance;

    [SerializeField] levelmanager levelManager;
    public bool level1Unlocked = true;
    public bool level2Unlocked = false;
    public bool level3Unlocked = false;
    public bool level4Unlocked = false;
    public bool level5Unlocked = false;
    [SerializeField]Button Level2Button;
    [SerializeField] Button Level3Button;
    [SerializeField] Button Level4Button;
    [SerializeField] Button Level5Button;
    public Sprite level3Img;
    public Sprite level2Img;
    public Sprite level4Img;
    public Sprite level5Img;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
}
