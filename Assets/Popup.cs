using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    //Create Image Popups and should go to the next popup when tapped

    [Tooltip("Assign the sequence of popup GameObjects here in the order they should appear.")]
    [SerializeField]
    private List<GameObject> popupSequence;
    [SerializeField] GameObject[] balls; 

    private int currentIndex = -1;

    //private void OnEnable()
    //{
    //    // Subscribe to the static onTap event from our TouchController
    //    TouchController.onTap += HandleTap;
    //}

    //private void OnDisable()
    //{
    //    // It's crucial to unsubscribe when the object is disabled or destroyed
    //    // to prevent errors and memory leaks.
    //    TouchController.onTap -= HandleTap;
    //}

    void Start()
    {
        // Ensure all popups are hidden initially
        foreach (var popup in popupSequence)
        {
            if (popup != null)
            {
                popup.SetActive(false);
            }
        }

        // If there are any popups in the sequence, show the first one
        if (popupSequence != null && popupSequence.Count > 0)
        {
            currentIndex = 0;
            Time.timeScale = 0; // Pause the game when showing popups
            popupSequence[currentIndex].SetActive(true);
        }
    }

    void Update()
    {
        // Detect a left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    /// <summary>
    /// This method is called by the TouchController's onTap event.
    /// It handles the logic for advancing to the next popup.
    /// </summary>
    //private void HandleTap(object sender, TouchGesture gesture)
    //{
    //    // If there are no popups or we've finished the sequence, do nothing.
    //    if (currentIndex < 0 || currentIndex >= popupSequence.Count)
    //    {
    //        return;
    //    }

    //    // Hide the current popup
    //    popupSequence[currentIndex].SetActive(false);

    //    // Advance to the next index
    //    currentIndex++;

    //    // Check if there is a next popup to show
    //    if (currentIndex < popupSequence.Count)
    //    {
    //        // Show the new current popup
    //        popupSequence[currentIndex].SetActive(true);
    //    }
    //    else
    //    {
    //        Time.timeScale = 1; // Resume the game when all popups are done
    //    }
    //}

    //Mouse Click
    private void HandleClick()
    {
        // If there are no popups or we've finished the sequence, do nothing
        if (currentIndex < 0 || currentIndex >= popupSequence.Count)
            return;

        // Hide the current popup
        popupSequence[currentIndex].SetActive(false);

        // Move to next
        currentIndex++;

        if (currentIndex < popupSequence.Count)
        {
            // Show next popup
            popupSequence[currentIndex].SetActive(true);
        }
        else
        {
            // End of sequence — resume the game
            Time.timeScale = 1;
        }
    }

}