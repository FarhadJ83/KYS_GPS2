using UnityEngine;
using System.Collections;

public class BallIntro : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private CharacterMovement cm;

    private SphereCollider col;

    private bool isStarting = true;

    private void Start()
    {
        col = GetComponent<SphereCollider>();

        if (cm != null) 
            cm.enabled = false;

        if (col != null)
            col.enabled = false;

        Debug.Log("turn off cm and col");

        isStarting = true;

        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(3f);

        yield return new WaitUntil(() => popupPanel == null || !popupPanel.activeSelf);

        if (Vector3.Distance(transform.position, spawnPoint.position) > 0.05f)
        {
            while (Vector3.Distance(transform.position, spawnPoint.position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = spawnPoint.position;
            Debug.Log("Finish intro");
        }

        if (cm != null)
            cm.enabled = true;

        if (col != null)
            col.enabled = true;

        isStarting = false;

    }
}
