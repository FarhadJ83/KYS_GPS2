using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
//using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] private LayerMask gateLayer;
    [SerializeField] private LayerMask rapidLayer;
    [SerializeField] public float raycastPadding = 0.01f;
    public int swipeCounter;

    private bool justTeleported = false;
    public Vector3 lastMoveDir { get; set; }
    public bool JustTeleported => justTeleported;
    public bool isMoving = false;

    private Animator animator;

    //Mouse Movement
    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;
    private bool isDragging = false;
    private float swipeThreshold = 50f;

    public void Start()
    {
        swipeCounter = 0;
        animator = GetComponent<Animator>();
    }

    //private void OnEnable()
    //{
    //    TouchController.onSwipe += OnSwipeMoveBall;
    //}

    //private void OnDisable()
    //{
    //    TouchController.onSwipe -= OnSwipeMoveBall;
    //}

    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level_1" && SceneManager.GetActiveScene().name != "Level_2")
        {
            UpdateInverseButton();
        }

        DetectMouseSwipe();
    }

    public void assignSwipe(int sc)
    {
        swipeCounter = sc;
    }

    private void UpdateInverseButton()
    {
        //Button InverseButton = GameObject.Find("InverseButton").GetComponent<Button>();
        //if (isMoving && InverseButton != null)
        //{
        //    InverseButton.interactable = false;
        //}
        //else if (!isMoving && InverseButton != null)
        //{
        //    InverseButton.interactable = true;
        //}

        //Mouse Movement
        Button InverseButton = GameObject.Find("InverseButton").GetComponent<Button>();
        if (InverseButton != null)
            InverseButton.interactable = !isMoving;
    }

    private void OnSwipeMoveBall(object sender, TouchGesture e)
    {
        if (isMoving) return;

        Vector2 swipeDir = (e.ScreenPosition - e.StartScreenPosition).normalized;
        Vector3 moveDir = (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
            ? (swipeDir.x > 0 ? Vector3.right : Vector3.left)
            : (swipeDir.y > 0 ? Vector3.forward : Vector3.back);

        lastMoveDir = moveDir;

        if (ball != null)
        {
            Vector3 origin = ball.position;
            Vector3 targetPos = GetTargetPosition(origin, moveDir);
            StartCoroutine(SmoothMoveToWall(ball, targetPos));
        }
    }

    private void DetectMouseSwipe()
    {
        // On mouse press
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
            isDragging = true;
        }

        // On mouse release
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            mouseEndPos = Input.mousePosition;
            isDragging = false;

            Vector2 swipe = mouseEndPos - mouseStartPos;

            if (swipe.magnitude < swipeThreshold)
                return; // too small, ignore

            Vector2 swipeDir = swipe.normalized;
            Vector3 moveDir = (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
                ? (swipeDir.x > 0 ? Vector3.right : Vector3.left)
                : (swipeDir.y > 0 ? Vector3.forward : Vector3.back);

            if (!isMoving)
            {
                lastMoveDir = moveDir;
                if (ball != null)
                {
                    Vector3 origin = ball.position;
                    Vector3 targetPos = GetTargetPosition(origin, moveDir);
                    StartCoroutine(SmoothMoveToWall(ball, targetPos));
                }
            }
        }
    }

    /// <summary>
    /// Calculates the final target position for a move, considering walls, gates, and rapids.
    /// </summary>
    public Vector3 GetTargetPosition(Vector3 origin, Vector3 moveDir)
    {
        // Immediate wall check (1 unit ahead)
        if (Physics.Raycast(origin, moveDir, out RaycastHit wallCheck, 1, wallLayer))
        {
            return origin; // Blocked immediately
        }

        // Gates
        Ray ray = new Ray(origin, moveDir);
        RaycastHit[] gateHits = Physics.RaycastAll(ray, Mathf.Infinity, gateLayer);
        Array.Sort(gateHits, (a, b) => a.distance.CompareTo(b.distance));

        // Wall
        bool hitWall = Physics.Raycast(ray, out RaycastHit wallHit, Mathf.Infinity, wallLayer);
        float wallDist = hitWall ? wallHit.distance : Mathf.Infinity;

        // Rapids
        int rapidLayerMask = 1 << LayerMask.NameToLayer("Rapid");
        RaycastHit[] rapidHits = Physics.RaycastAll(ray, Mathf.Infinity, rapidLayerMask);

        Array.Sort(rapidHits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit rapidHit in rapidHits)
        {
            WaterRapidsDirection rapid = rapidHit.collider.GetComponent<WaterRapidsDirection>();
            if (rapid != null)
            {
                float dot = Vector3.Dot(moveDir, rapid.direction);
                Debug.Log($"[Rapids Raycast] Hit {rapidHit.collider.name} | Dot={dot}");

                if (dot < 0)
                    return rapidHit.point - moveDir * raycastPadding;
            }
        }

        // Gate color check
        BallColorState ballState = ball.GetComponent<BallColorState>();
        foreach (RaycastHit gateHit in gateHits)
        {
            bool isBlackGate = gateHit.collider.CompareTag("BlackGate");
            bool isMismatch = ballState != null && ballState.isBlack != isBlackGate;

            if (isMismatch && gateHit.distance < wallDist)
            {
                return gateHit.point - moveDir * raycastPadding;
            }
        }

        // Default: wall or far away
        return hitWall
            ? wallHit.point - moveDir * raycastPadding
            : origin + moveDir * 10f;
    }

    private IEnumerator SmoothMoveToWall(Transform obj, Vector3 targetPos)
    {
        isMoving = true;

        if (Vector3.Distance(obj.position, targetPos) > 0.01f)
            swipeCounter++;

        animator.SetBool("IsMoving", true);

        while (Vector3.Distance(obj.position, targetPos) > 0.01f)
        {
            if (lastMoveDir != Vector3.zero)
                obj.rotation = Quaternion.LookRotation(lastMoveDir);

            obj.position = Vector3.MoveTowards(obj.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        obj.position = targetPos;
        animator.SetBool("IsMoving", false);
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
