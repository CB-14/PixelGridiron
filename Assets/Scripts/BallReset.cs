using UnityEngine;

public class BallReset : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody2D rb;

    void Start()
    {
        initialPosition = transform.position;  // Store where the ball starts
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetBall()
    {
        transform.position = initialPosition;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
 