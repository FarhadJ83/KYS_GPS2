using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask wallLayer; // Set this to include only wall objects
    [SerializeField] private float raycastPadding = 0.01f;

    private bool isMoving = false;

    private void OnEnable()
    {
        TouchController.onSwipe += OnSwipeMoveBall;
    }

    private void OnDisable()
    {
        TouchController.onSwipe -= OnSwipeMoveBall;
    }

    private void OnSwipeMoveBall(object sender, TouchGesture e)
    {
        if (isMoving) return;

        Vector2 swipeDir = (e.ScreenPosition - e.StartScreenPosition).normalized;

        Vector3 moveDir = Vector3.zero;

        if (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
            moveDir = swipeDir.x > 0 ? Vector3.right : Vector3.left;
        else
            moveDir = swipeDir.y > 0 ? Vector3.forward : Vector3.back;

        if (ball != null)
        {
            // Raycast to check how far until wall
            Ray ray = new Ray(ball.position, moveDir);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, wallLayer))
            {
                Vector3 targetPos = hit.point - moveDir * raycastPadding;
                StartCoroutine(SmoothMoveToWall(ball, targetPos));
            }
        }
    }

    private IEnumerator SmoothMoveToWall(Transform obj, Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(obj.position, targetPos) > 0.01f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        obj.position = targetPos;
        isMoving = false;
    }
}
