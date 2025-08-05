using UnityEngine;

public enum RouteType
{
    Slant,
    Go,
    Post,
    Out
}

public class ReceiverRoute : MonoBehaviour
{
    [Header("Route Settings")]
    public RouteType routeType = RouteType.Slant;
    public float moveSpeed = 3.5f;
    public float reachDistance = 0.2f;

    [Header("Animation (optional)")]
    public Animator animator;

    private Vector3[] routePoints;
    private int currentPoint = 0;
    private bool routeComplete = false;
    private bool snapped = false;

    private SpriteRenderer spriteRenderer;
    private float fixedZ;

    void Start()
    {
        routePoints = RouteFactory.GenerateRoute(transform.position, routeType);
        spriteRenderer = GetComponent<SpriteRenderer>();
        fixedZ = transform.position.z;

        // Debug: confirm initial visibility
        if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            Debug.LogWarning($"{gameObject.name} started with disabled SpriteRenderer! Enabling...");
            spriteRenderer.enabled = true;
        }
    }

    void Update()
    {
        if (!snapped || routeComplete || routePoints == null || routePoints.Length == 0) return;

        // Prevent disappearing by clamping Z
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);

        if (currentPoint >= routePoints.Length)
        {
            routeComplete = true;
            if (animator != null)
                animator.SetBool("isRunning", false);
            return;
        }

        // Safety: make sure SpriteRenderer stays enabled
        if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            Debug.LogWarning($"{gameObject.name} SpriteRenderer DISABLED during route! Re-enabling.");
            spriteRenderer.enabled = true;
        }

        Vector3 target = routePoints[currentPoint];
        target.z = fixedZ; // Lock Z

        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Flip sprite
        if (direction.x > 0)
            transform.localScale = new Vector3(0.14f, 0.165f, 1f);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-0.14f, 0.165f, 1f);

        // Animation
        if (animator != null)
            animator.SetBool("isRunning", true);

        if (Vector3.Distance(transform.position, target) < reachDistance)
            currentPoint++;
    }

    public void StartRoute()
    {
        snapped = true;
        routeComplete = false;
        currentPoint = 0;
        if (routePoints != null && routePoints.Length > 0)
        {
            // Force reset to start, clamp Z
            Vector3 start = routePoints[0];
            transform.position = new Vector3(start.x, start.y, fixedZ);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (routePoints == null || routePoints.Length < 2)
            return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < routePoints.Length - 1; i++)
        {
            Gizmos.DrawLine(routePoints[i], routePoints[i + 1]);
        }
    }
}
