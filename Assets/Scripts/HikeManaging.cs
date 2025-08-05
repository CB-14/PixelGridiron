using UnityEngine;
using TMPro;
using System.Collections;

public class SnapManager : MonoBehaviour
{
    [Header("References")]
    public Transform ball;
    public Transform quarterback;
    public Transform BallHolder;
    public ReceiverRoute[] receivers;
    public Animator qbAnimator;
    public Transform losMarker;
    public DefenderAI[] defenders;


    [Header("Snap Settings")]
    public float snapSpeed = 3f;

    [Header("Dropback Settings")]
    public float dropbackDistance = 0.5f;
    public float dropbackSpeed = 1.4f;

    [Header("Throw Settings")]
    public float throwForceMultiplier = 5f;
    public float maxForce = 20f;

    [Header("Aiming")]
    public LineRenderer aimLine;
    public float aimLineLength = 2f;

    // 👇 Global playStarted flag for defenders
    public static bool playStarted = false;

    private Vector3 qbDropTarget;
    private bool isSnapping = false;
    private bool snapped = false;
    private bool isDroppingBack = false;
    private bool dropbackComplete = false;
    private bool hasThrown = false;
    private bool hikeStarted = false;

    private Rigidbody2D ballRb;
    private Rigidbody2D qbRb;

    private Vector2 dragStart;
    private bool isDragging = false;
    private bool isThrowReadyAnimPlaying = false;

    void Start()
    {
        if (defenders == null || defenders.Length == 0)
    {
    defenders = FindObjectsOfType<DefenderAI>();
    Debug.Log($"Auto-found {defenders.Length} defenders.");
    }

        if (ball != null)
        {
            ballRb = ball.GetComponent<Rigidbody2D>();
            ballRb.isKinematic = true;
            ballRb.linearVelocity = Vector2.zero;
        }
          if (receivers == null || receivers.Length == 0)
    {
        receivers = FindObjectsOfType<ReceiverRoute>();
        Debug.Log($"Auto-found {receivers.Length} receivers.");
    }

        if (quarterback != null)
        {
            qbRb = quarterback.GetComponent<Rigidbody2D>();
            qbRb.gravityScale = 0f;
            qbRb.freezeRotation = true;
            qbRb.linearVelocity = Vector2.zero;
        }

        if (qbAnimator != null)
        {
            qbAnimator.enabled = false;
            qbAnimator.Rebind();
            qbAnimator.Update(0f);
        }

        if (aimLine != null)
        {
            aimLine = GameObject.Find("aimLine").GetComponent<LineRenderer>();
            aimLine.enabled = true;
            aimLine.positionCount = 0;
        }

        Debug.Log("SnapManager initialized");
    }

    void Update()
    {
        LogState();

        if (playStarted && dropbackComplete && quarterback != null && losMarker != null)
    {
      if (quarterback.position.x > losMarker.position.x)
    {
        foreach (var defender in defenders)
        {
            if (defender != null)
                defender.StartChasing();
        }
    }
}

        if (!hikeStarted && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Hike triggered!");
            hikeStarted = true;
            isSnapping = true;

            // 👇 Trigger global play start
            playStarted = true;

            if (qbAnimator != null)
            {
                qbAnimator.enabled = true;
                qbAnimator.Rebind();
                qbAnimator.Update(0f);
                Debug.Log("QB Animator enabled and rebound");
            }
        }

        if (!hikeStarted) return;

        // (rest of your existing code continues here...)

        if (isSnapping && ball != null && quarterback != null)
        {
            ball.position = Vector3.MoveTowards(ball.position, quarterback.position, snapSpeed * Time.deltaTime);

            if (Vector3.Distance(ball.position, quarterback.position) < 0.05f)
            {
                Debug.Log("Snap complete!");
                isSnapping = false;
                snapped = true;

                if (BallHolder != null)
                {
                    ball.SetParent(BallHolder);
                    ball.localPosition = Vector3.zero;
                    ball.localRotation = Quaternion.identity;
                }

                qbDropTarget = quarterback.position + Vector3.left * dropbackDistance;
                isDroppingBack = true;
                dropbackComplete = false;

                if (qbAnimator != null && qbAnimator.enabled)
                {
                    qbAnimator.ResetTrigger("Dropback");
                    qbAnimator.SetTrigger("Dropback");
                    Debug.Log("QB Dropback animation triggered");
                }

                foreach (ReceiverRoute route in receivers)
                {
                    if (route != null)
                    {
                        route.StartRoute();
                        Debug.Log("Receiver route started");
                    }
                }
            }
        }

        if (isDroppingBack)
        {
            if (qbRb != null)
                qbRb.MovePosition(Vector2.MoveTowards(qbRb.position, qbDropTarget, dropbackSpeed * Time.deltaTime));
            else
                quarterback.position = Vector3.MoveTowards(quarterback.position, qbDropTarget, dropbackSpeed * Time.deltaTime);

            if (Vector3.Distance(quarterback.position, qbDropTarget) < 0.01f)
            {
                Debug.Log("Dropback complete");
                isDroppingBack = false;
                dropbackComplete = true;
            }
        }

        // Drag to aim and throw
        if (snapped && dropbackComplete && !hasThrown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Start dragging/aiming...");
                isDragging = true;

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
                dragStart = Camera.main.ScreenToWorldPoint(mousePos);

                if (qbAnimator != null && qbAnimator.enabled && !isThrowReadyAnimPlaying)
                {
                    qbAnimator.ResetTrigger("ReleaseThrow");
                    qbAnimator.SetTrigger("StartThrow");
                    isThrowReadyAnimPlaying = true;
                    Debug.Log("Throw ready animation triggered");
                }
            }

            if (isDragging && aimLine != null && Input.GetMouseButton(0))
            {
                aimLine.enabled = true;

                Vector3 currentMouse = Input.mousePosition;
                currentMouse.z = Mathf.Abs(Camera.main.transform.position.z);
                Vector2 dragCurrent = Camera.main.ScreenToWorldPoint(currentMouse);

                Vector2 dragVector = dragStart - dragCurrent;
                Vector2 throwDirection = dragVector.normalized;
                float throwStrength = Mathf.Clamp(dragVector.magnitude * throwForceMultiplier, 0, maxForce);
                Vector2 velocity = throwDirection * throwStrength;

                DrawAimLine(ball.position, velocity);
            }
            else if (!isDragging && aimLine != null)
            {
                aimLine.enabled = false;
                aimLine.positionCount = 0;
            }

            if (isDragging && Input.GetMouseButtonUp(0))
            {
                Debug.Log("Throw released");
                isDragging = false;

                if (aimLine != null)
                {
                    aimLine.positionCount = 0;
                    aimLine.enabled = false;
                }

                if (qbAnimator != null && qbAnimator.enabled)
                {
                    qbAnimator.ResetTrigger("StartThrow");
                    qbAnimator.SetTrigger("ReleaseThrow");
                    isThrowReadyAnimPlaying = false;
                    Debug.Log("Throw animation triggered");
                }

                ThrowFromDrag();
            }
        }
    }

    void DrawAimLine(Vector2 startPosition, Vector2 initialVelocity)
    {
        int steps = 30;
        float timeStep = 0.05f;
        Vector2 gravity = Physics2D.gravity;

        aimLine.positionCount = steps;

        for (int i = 0; i < steps; i++)
        {
            float t = i * timeStep;
            Vector2 pos = startPosition + initialVelocity * t + 0.5f * gravity * t * t;
            aimLine.SetPosition(i, pos);
        }
    }

    void ThrowFromDrag()
    {
        if (ball == null || ballRb == null) return;

        Vector3 dragEndPos = Input.mousePosition;
        dragEndPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector2 dragEnd = Camera.main.ScreenToWorldPoint(dragEndPos);

        Vector2 dragVector = dragStart - dragEnd;
        Vector2 throwDirection = dragVector.normalized;
        float throwStrength = Mathf.Clamp(dragVector.magnitude * throwForceMultiplier, 0, maxForce);

        ball.SetParent(null);
        ballRb.isKinematic = false;
        ballRb.linearVelocity = throwDirection * throwStrength;

        snapped = false;
        hasThrown = true;

        Debug.Log($"Ball Thrown! Direction: {throwDirection}, Strength: {throwStrength}");
    }


   void LogState()
    {
        Debug.Log($"[STATE] hikeStarted: {hikeStarted}, isSnapping: {isSnapping}, snapped: {snapped}, isDroppingBack: {isDroppingBack}, dropbackComplete: {dropbackComplete}, hasThrown: {hasThrown}, isDragging: {isDragging}");
    }

    public static void ResetPlay()
    {
    playStarted = false;

    PlayerPositionReset prm = FindObjectOfType<PlayerPositionReset>();
if (prm != null)
    prm.ResetPlayersRelativeToLOS();

    // Find the SnapManager instance to reset instance variables
    SnapManager instance = FindObjectOfType<SnapManager>();
    if (instance != null)
    {
        instance.hikeStarted = false;
        instance.isSnapping = false;
        instance.snapped = false;
        instance.isDroppingBack = false;
        instance.dropbackComplete = false;
        instance.hasThrown = false;

        // Reset ball position and state if possible
        if (instance.ball != null && instance.BallHolder != null)
        {
            instance.ball.SetParent(instance.BallHolder);
            instance.ball.localPosition = Vector3.zero;
            instance.ball.localRotation = Quaternion.identity;

            Rigidbody2D ballRb = instance.ball.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                ballRb.isKinematic = true;
                ballRb.linearVelocity = Vector2.zero;
            }
        }
    }
}
}