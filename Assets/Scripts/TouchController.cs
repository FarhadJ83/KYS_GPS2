using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchGesture
{

    public int TouchId { get; }
    public Vector2 StartScreenPosition { get; }
    public Vector2 ScreenPosition { get; private set; }
    public Vector2 PreviousScreenPosition { get; private set; }
    public double StartTime { get; }
    public double Time { get; private set; }
    public double PreviousTime { get; private set; }
    public double TravelDistance { get; private set; }
    public double Sameness { get; private set; }
    private Vector2 cumulativeMoveDirection;
    private int sampleSize;

    public bool IsTap { get; private set; }
    public bool IsSwipe { get; private set; }

    private bool moving = false;
    private const float MIN_MOVE_THRESHOLD = 10.0f;

    public double MovementDirection { get; private set; }
    public static object Direction { get; internal set; }

    public TouchGesture(int touchId, Vector2 startScreenPosition, double startTime)
    {
        TouchId = touchId;
        StartScreenPosition = startScreenPosition;
        // When object is just created, current position and
        // previous position is the same as start position
        ScreenPosition = startScreenPosition;
        PreviousScreenPosition = startScreenPosition;
        StartTime = startTime;
    }

    public Vector2 GetSwipeDirection()
    {
        return (ScreenPosition - StartScreenPosition).normalized;
    }

    internal void SubmitPoint(Vector2 currentScreenPosition, double currentTime)
    {
        // Assign 'current position' to 'previous position'
        PreviousScreenPosition = ScreenPosition;

        // Assign 'screenPosition' to 'current position'
        ScreenPosition = currentScreenPosition;

        // Assign 'current time' to 'previous time'
        PreviousTime = Time;

        // Assign 'currentTine' to 'current time'
        Time = currentTime;

        // If moving is false
        if (!moving)
        {
            // If distance is 'current position' to 'start position'
            // is above MIN_MOVE_THRESHOLD, set 'moving' to true
            float distance = Vector2.Distance(ScreenPosition, StartScreenPosition);
            moving = distance > MIN_MOVE_THRESHOLD;
        }
        else
        {
            // 1. find the movement direction from previous to current
            Vector2 moveDirection = ScreenPosition - StartScreenPosition;

            float moveDist = moveDirection.magnitude; // get the length for some calculation

            // normalize direction for next movement
            // we calculated magnitude before so we can just simply scalar division
            moveDirection /= moveDist;

            // add moveDist to travelDist
            TravelDistance += moveDist;

            // 2. accumulate it with previously tracked movement direction
            cumulativeMoveDirection += moveDirection;

            // 3. find out how similar
            Sameness = Vector2.Dot(moveDirection, cumulativeMoveDirection / sampleSize);
        }
        // find movement direction
    }

    internal void FinalizeGesture()
    {
        double timeDelta = Time - StartTime;
        // if moving
        if (moving)
        {
            // check if time passed less than SWIPE_DURATION
            if (timeDelta < 0.5)
            {
                bool acceptSwipe = Sameness > 0.9f;

                // then a different check (later discuss)
                if (acceptSwipe)
                {
                    // if all condition met, set IsSwipe to true
                    IsSwipe = true;
                }
            }
        }
        else
        {
            // otherwise
            // check if time passed less than TAP_DURATION
            // if so, set IsTap is true
            if (timeDelta < 0.2)
            {
                IsTap = true;
            }
        }

    }
}

public class TouchController : MonoBehaviour
{
    private static TouchController instance;

    // Records of any active touches and over their individual lifetime
    private readonly Dictionary<int, TouchGesture> gestureCollection = new();

    public static EventHandler<TouchGesture> onPress;
    public static EventHandler<TouchGesture> onRelease;
    public static EventHandler<TouchGesture> onUpdate;
    public static EventHandler<TouchGesture> onTap;
    public static EventHandler<TouchGesture> onSwipe;

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

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            EnhancedTouchSupport.Disable();
        }
    }

    void Update()
    {
        var activeTouches = Touch.activeTouches;

        foreach (var touch in activeTouches)
        {
            // Check the phase
            TouchPhase phase = touch.phase;

            // If began
            if (phase == TouchPhase.Began)
            {
                // Create new TouchGesture and add to gestureCollection
                TouchGesture gesture = new TouchGesture(touch.touchId, touch.startScreenPosition, touch.startTime);

                // If, for some reason already exist, simply replace (rare case)
                gestureCollection[touch.touchId] = gesture;

                // Invoke relevant event
                onPress?.Invoke(this, gesture);
                Debug.Log($"Touch Began, touchId : {touch.touchId}");
            }
            // Otherwise
            else
            {
                // If cannot find the gesture in gestureCollection, skip this iteration
                // and continue to next iteration
                if (!gestureCollection.TryGetValue(touch.touchId, out TouchGesture gesture))
                {
                    continue;
                }

                // gesture is valid here

                // If moving, add the new position to be tracked
                if (phase == TouchPhase.Moved)
                {
                    gesture.SubmitPoint(touch.screenPosition, Time.realtimeSinceStartupAsDouble);

                    // Invoke relevant event, currently nothing needed
                    onUpdate?.Invoke(this, gesture);

                    // If you want to immediately trigger potential swipe..
                    // this is where you should invoke the event
                    // e.g. Subway surfer swipe left and right
                    // note: this requires you to split IsSwipe check from
                    // finalizeGesture so you only check for IsSwipe

                }
                // Internal TouchGesture processing happens here
                // If stationary, do nothing?
                // If ended, or cancel
                else if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
                {
                    onRelease?.Invoke(this, gesture);

                    // Add the new position to be tracked
                    gesture.SubmitPoint(touch.screenPosition, Time.realtimeSinceStartupAsDouble);
                    gesture.FinalizeGesture();

                    onUpdate?.Invoke(this, gesture);
                    onRelease?.Invoke(this, gesture);

                    // Internal TouchGesture processing happens here
                    // Invoke relevant event
                    if (gesture.IsTap)
                    {
                        onTap?.Invoke(this, gesture);
                    }
                    else if (gesture.IsSwipe)
                    {
                        onSwipe?.Invoke(this, gesture);
                    }

                    // Remove the gesture from records. The touch is lifted, and its lifetime ended.
                    gestureCollection.Remove(touch.touchId);
                }
            }

        }
    }
}
