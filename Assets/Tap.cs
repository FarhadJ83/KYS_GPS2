using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        // We use the provided TouchController for tap input
        TouchController.onTap += HandleTap;
    }

    private void OnDisable()
    {
        TouchController.onTap -= HandleTap;
    }

    private void HandleTap(object sender, TouchGesture e)
    {
        // On tap, load the Menu Scene if the tap is not on a button

        if (IsTapOverButtonOrPanel(e))
            return;

        SceneManager.LoadScene("LevelsScene");
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


}
