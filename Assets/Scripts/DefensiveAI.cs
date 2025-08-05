using UnityEngine;

public enum DefenderRole
{
    Cornerback,
    Safety,
    Linebacker,
    DefensiveLine
}

[RequireComponent(typeof(Rigidbody2D))]
public class DefenderAI : MonoBehaviour
{
    [Header("Role Settings")]
    public DefenderRole role;

    [Header("Assignments")]
    public Transform assignedReceiver; // For CBs and Safeties
    public Transform quarterback;      // Assign QB here or find at runtime

    [Header("Movement Settings")]
    public float moveSpeed = 4.5f;
    public float stoppingDistance = 0.5f;

    [Header("Sack Settings")]
    public SackManager sackManager;    // Assign this in inspector or at runtime

    private Rigidbody2D rb;

    [Header("Chasing Settings")]
    public bool isChasing = false;
    public float chaseSpeed = 7f; // Speed when chasing QB

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (quarterback == null)
            quarterback = GameObject.FindGameObjectWithTag("QB")?.transform;

        // Set default speeds per role if not set manually
        switch (role)
        {
            case DefenderRole.DefensiveLine:
                moveSpeed = 2f;
                break;

            case DefenderRole.Linebacker:
                moveSpeed = 3f;
                break;

            case DefenderRole.Cornerback:
            case DefenderRole.Safety:
                moveSpeed = 6.5f;
                break;
        }
    }

    void Update()
    {
        if (!SnapManager.playStarted)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isChasing && quarterback != null)
        {
            ChaseQuarterback();
            return; // Skip other behaviors while chasing
        }

        switch (role)
        {
            case DefenderRole.Cornerback:
                ShadowReceiver();
                break;
            case DefenderRole.DefensiveLine:
                RushQuarterback();
                break;
            case DefenderRole.Safety:
                DeepCoverage();
                break;
            case DefenderRole.Linebacker:
                ReadAndReact();
                break;
        }
    }

    // Begin chase behavior toward QB
    public void StartChasing()
    {
        isChasing = true;
    }

    // Stop chasing and reset movement
    public void StopChasing()
    {
        isChasing = false;
        rb.linearVelocity = Vector2.zero;
    }

    // Actively move toward QB
    void ChaseQuarterback()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, quarterback.position, chaseSpeed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    void ShadowReceiver()
    {
        if (assignedReceiver == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, assignedReceiver.position);
        if (distance > stoppingDistance)
        {
            Vector2 direction = (assignedReceiver.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void RushQuarterback()
    {
        if (quarterback == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (quarterback.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    void DeepCoverage()
    {
        if (assignedReceiver != null)
        {
            Vector2 direction = (assignedReceiver.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            Vector2 zoneTarget = new Vector2(transform.position.x, transform.position.y - 10f);
            Vector2 direction = (zoneTarget - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * (moveSpeed * 0.7f);
        }
    }

    void ReadAndReact()
    {
        rb.linearVelocity = Vector2.up * (moveSpeed * 0.3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (role == DefenderRole.DefensiveLine && other.CompareTag("QB"))
        {
            if (sackManager != null)
            {
                sackManager.OnQBSacked();
            }
            else
            {
                Debug.LogWarning("SackManager not assigned in DefenderAI!");
            }
        }
    }
}
