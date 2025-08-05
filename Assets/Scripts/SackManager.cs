using UnityEngine;
using System.Collections;

public class SackManager : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D qbRb;           // QB Rigidbody2D (assign in inspector or auto-grab)
    public Transform losMarker;        // Line of Scrimmage marker (assign in inspector)

    [Header("Settings")]
    public float restartDelay = 3f;    // Delay before restarting play

    private bool isSacked = false;
    private Vector3 sackPosition;

    void Start()
    {
        if (qbRb == null)
            qbRb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isSacked) return;

        if (other.CompareTag("Defender"))
        {
            HandleSack();
        }
    }

    public void OnQBSacked()
    {
        if (!isSacked)
            HandleSack();
    }

    private void HandleSack()
    {
        isSacked = true;
        sackPosition = transform.position;

        // Freeze QB movement
        qbRb.linearVelocity = Vector2.zero;
        qbRb.isKinematic = true;

        // Rotate QB sideways (simulate fall)
        transform.rotation = Quaternion.Euler(0, 0, 90f);

        // Start reset coroutine
        StartCoroutine(RestartPlayAfterDelay());
    }

    IEnumerator RestartPlayAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);

        // Move LOS marker to sack position (x only)
        if (losMarker != null)
        {
            Vector3 newPos = losMarker.position;
            newPos.x = sackPosition.x;
            losMarker.position = newPos;
        }

        PlayerPositionReset resetScript = FindObjectOfType<PlayerPositionReset>();
    if (resetScript != null)
    {
      resetScript.ResetPlayersRelativeToLOS();
    }
    else
    {
    Debug.LogWarning("PlayerPositionReset script not found in the scene!");
    }


        // Reset QB rotation and slightly behind sack spot
        transform.rotation = Quaternion.identity;
        transform.position = losMarker.position + new Vector3(-0.5f, 0, 0);
        qbRb.isKinematic = false;

        isSacked = false;

        // Reset snap and play state
        SnapManager.ResetPlay(); // Ensure your SnapManager has this method
    }
}
