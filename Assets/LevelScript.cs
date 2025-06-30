using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public bool level1Unlocked = true;
    public bool level2Unlocked = false;

    LevelScript instance;
    public void Awake()
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
}
