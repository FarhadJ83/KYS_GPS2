using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private GameObject startingTransition;
    [SerializeField] private GameObject endTransition;

    private void Start()
    {
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
            endTransition.SetActive(true);
            yield return new WaitForSeconds(1f);
            endTransition.SetActive(false);

    }
}
