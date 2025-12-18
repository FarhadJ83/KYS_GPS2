using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class audiosettings : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle vibrationToggle;
    private AudioSource[] audioSources;

    private void Awake()
    {

    }
    private void Start() 
    {
        volumeSlider = Camera.main.GetComponent<levelmanager>().settingsPanel.gameObject.transform.Find("Volume")?.GetComponent<Slider>();
        vibrationToggle = Camera.main.GetComponent<levelmanager>().settingsPanel.gameObject.transform.Find("VibrationToggle")?.GetComponent<Toggle>();
        if (audioSource == null)
        {
            audioSource = GameObject.Find("AudioManager")?.GetComponent<AudioSource>();
        }
        if (audioSources == null)
        {
            audioSources = GameObject.Find("AudioManager2").GetComponent<ambientsounds>().audioSources;
        }
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            volumeSlider.value = audioSource.volume;
            for (int i = 0; i < audioSources.Length; i++)
            {
                volumeSlider.value = audioSources[i].volume;
            }
        }
        if (vibrationToggle != null)
        {
            vibrationToggle.onValueChanged.AddListener(SetVibration);
            vibrationToggle.isOn = PlayerPrefs.GetInt("Vibration", 1) == 1; // Default to on
        }
    }

    private void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
        if (audioSources != null)
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = volume;
            }
        }
    }

    private void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Vibration", isOn ? 1 : 0);
        // Here you can add code to enable or disable vibration in your game
        if (isOn)
        {
            Debug.Log("Vibration enabled");

            #if UNITY_ANDROID || UNITY_IOS
                    Handheld.Vibrate();
            #else
                        Debug.Log("Vibration not supported on this platform.");
            #endif
        }
        else
        {
            Debug.Log("Vibration disabled");
        }
    }
}
