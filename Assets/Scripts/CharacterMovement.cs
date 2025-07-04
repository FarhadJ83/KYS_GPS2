﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.UI.Image;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] private LayerMask gateLayer;
    [SerializeField] public float raycastPadding = 0.01f;

    private bool justTeleported = false;

    public Vector3 lastMoveDir { get; set; }
    public bool JustTeleported => justTeleported;

    public bool isMoving = false;

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
        Vector3 moveDir = (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y)) ? (swipeDir.x > 0 ? Vector3.right : Vector3.left) : (swipeDir.y > 0 ? Vector3.forward : Vector3.back);

        lastMoveDir = moveDir;

        if (ball != null)
        {
            Vector3 origin = ball.position;

            // Wall raycast
            bool hitWall = Physics.Raycast(origin, moveDir, out RaycastHit wallHit, Mathf.Infinity, wallLayer);
            float wallDist = hitWall ? wallHit.distance : Mathf.Infinity;

            // Gate raycast
            bool hitGate = Physics.Raycast(origin, moveDir, out RaycastHit gateHit, Mathf.Infinity, gateLayer);
            float gateDist = hitGate ? gateHit.distance : Mathf.Infinity;

            // If gate is closer than wall
            if (hitGate && gateDist < wallDist)
            {
                GameObject gateObj = gateHit.collider.gameObject;
                BallColorState ballState = ball.GetComponent<BallColorState>();

                bool isBlackGate = gateObj.CompareTag("BlackGate");
                bool isMismatch = ballState != null && ballState.isBlack != isBlackGate;

                if (isMismatch)
                {
                    // Ball is blocked by mismatched gate
                    Vector3 stopBeforeGate = gateHit.point - moveDir * raycastPadding;
                    StartCoroutine(SmoothMoveToWall(ball, stopBeforeGate));
                    return;
                }

                // Otherwise, fall through to wall (or max movement)
            }

            // Wall or matching gate is next
            Vector3 targetPos = hitWall
                ? wallHit.point - moveDir * raycastPadding
                : origin + moveDir * 10f; // fallback if no wall

            StartCoroutine(SmoothMoveToWall(ball, targetPos));
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

    public IEnumerator SendTo(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(ball.position, targetPos) > 0.01f)
        {
            ball.position = Vector3.MoveTowards(ball.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        ball.position = targetPos;
        isMoving = false;
    }

}
