using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void OnEnable()
    //{
    //    // We use the provided TouchController for tap input
    //    TouchController.onTap += HandleTap;
    //}

    //private void OnDisable()
    //{
    //    TouchController.onTap -= HandleTap;
    //}

    //private void HandleTap(object sender, TouchGesture e)
    //{
    //    // On tap, load the Menu Scene if the tap is not on a button

    //    if (IsTapOverButtonOrPanel(e))
    //        return;

    //    SceneManager.LoadScene("LevelsScene");
    //}

    void Update()
    {
        // Detect left mouse click
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Input.mousePosition;

            // Only load if not clicking on a UI button or panel
            if (!IsPointerOverButtonOrPanel(mousePos))
            {
                SceneManager.LoadScene("LevelsScene");
            }
        }
    }

    private bool IsPointerOverButtonOrPanel(Vector2 screenPos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null ||
                result.gameObject.CompareTag("Panel"))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTapOverButtonOrPanel(TouchGesture e)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = e.ScreenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null || result.gameObject.tag.Equals("Panel"))
            {
                return true;

            }
        }
        return false;
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Deletes all keys and values from PlayerPrefs
        Debug.LogWarning("PLAYER PROGRESS RESET!"); // Use a warning to make it stand out
    }

}
