using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ambientsounds : MonoBehaviour
{
    static ambientsounds instance;
    public AudioClip bidchirp;
    public AudioClip frog;
    public AudioClip water;
    public AudioClip wind;
    public AudioSource[] audioSources;
    bool isPlaying = false;
    int random;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LevelsScene")
        {
            if(!isPlaying)
            {
                random = Random.Range(0, 15);
                switch(random)
                {
                    case 0: StartCoroutine(play4Sounds()); break;
                    case 1: StartCoroutine(play3Sounds(bidchirp, frog, water)); break;
                    case 2: StartCoroutine(play3Sounds(bidchirp, frog, wind)); break;
                    case 3: StartCoroutine(play3Sounds(bidchirp, water, wind)); break;
                    case 4: StartCoroutine(play3Sounds(frog, water, wind)); break;
                    case 5: StartCoroutine(play2Sounds(bidchirp, frog)); break;
                    case 6: StartCoroutine(play2Sounds(bidchirp, water)); break;
                    case 7: StartCoroutine(play2Sounds(bidchirp, wind)); break;
                    case 8: StartCoroutine(play2Sounds(frog, water)); break;
                    case 9: StartCoroutine(play2Sounds(frog, wind)); break;
                    case 10: StartCoroutine(play2Sounds(water, wind)); break;
                    case 11: StartCoroutine(play1Sound(bidchirp)); break;
                    case 12: StartCoroutine(play1Sound(frog)); break;
                    case 13: StartCoroutine(play1Sound(water)); break;
                    case 14: StartCoroutine(play1Sound(wind)); break;
                }
            }
        }
    }

    IEnumerator play4Sounds()
    {
        isPlaying = true;
        Debug.Log("Playing all 4 sounds");
        audioSources[0].PlayOneShot(bidchirp);
        audioSources[1].PlayOneShot(frog);
        audioSources[2].PlayOneShot(water);
        audioSources[3].PlayOneShot(wind);
        yield return new WaitForSeconds(wind.length);
        isPlaying = false;
    }

    IEnumerator play1Sound(AudioClip ac)
    {
        isPlaying = true;
        Debug.Log("Playing 1 sound");
        audioSources[0].PlayOneShot(ac);
        yield return new WaitForSeconds(ac.length);
        isPlaying = false;
    }

    IEnumerator play2Sounds(AudioClip ac1, AudioClip ac2)
    {
        isPlaying = true;
        Debug.Log("Playing 2 sounds");
        audioSources[0].PlayOneShot(ac1);
        audioSources[1].PlayOneShot(ac2);
        yield return new WaitForSeconds(Mathf.Max(ac1.length, ac2.length));
        isPlaying = false;
    }

    IEnumerator play3Sounds(AudioClip ac1, AudioClip ac2, AudioClip ac3)
    {
        isPlaying = true;
        Debug.Log("Playing 3 sounds");
        audioSources[0].PlayOneShot(ac1);
        audioSources[1].PlayOneShot(ac2);
        audioSources[2].PlayOneShot(ac3);
        yield return new WaitForSeconds(Mathf.Max(ac1.length, Mathf.Max(ac2.length, ac3.length)));
        isPlaying = false;
    }
}
