using UnityEngine;
using UnityEngine.Rendering.UI;

public class InverseController : MonoBehaviour
{
    [HideInInspector] GameObject whiteEnvironment;
    [HideInInspector] GameObject blackEnvironment;
    public Color YangColor;
    public Color YinColor;
    GameObject[] whiteGates;
    GameObject[] blackGates;

    private bool isWhiteActive = true;

    public void Awake()
    {
        whiteEnvironment = GameObject.Find("Yang World");
        blackEnvironment = GameObject.Find("Yin World");
        whiteGates = GameObject.FindGameObjectsWithTag("WhiteGate");
        blackGates = GameObject.FindGameObjectsWithTag("BlackGate");
        if (blackEnvironment != null)
            blackEnvironment.SetActive(false);
        Camera.main.backgroundColor = YangColor; 
    }

    public void Update()
    {
        //if(whiteGates != null)
        //    foreach(GameObject gate in whiteGates)
        //    {
        //        // Have the y set at 0.13
        //        gate.transform.position = new Vector3(gate.transform.position.x, 0.13f, gate.transform.position.z);
        //        //Set z rotatioon to 180 and y rotation to -90
        //        gate.transform.rotation = Quaternion.Euler(0, -90, 180);
        //    }

        //if (blackGates != null)
        //    foreach (GameObject gate in blackGates)
        //    {
        //        // Have the y set at 0.35
        //        gate.transform.position = new Vector3(gate.transform.position.x, 0.35f, gate.transform.position.z);
        //        //Set z rotation to 180 and y rotation to -90
        //        gate.transform.rotation = Quaternion.Euler(0, -90, 0);
        //    }
    }

    public void InvertEnvironment()
    {
        isWhiteActive = !isWhiteActive;

        Camera.main.backgroundColor = isWhiteActive ? YangColor : YinColor;

        whiteEnvironment.SetActive(isWhiteActive);

        blackEnvironment.SetActive(!isWhiteActive);

    }
}
//8D8D8D