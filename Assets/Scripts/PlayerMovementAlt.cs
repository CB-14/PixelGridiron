using UnityEngine;

public class PlayerMovementAlt : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Prevent unwanted rotation on collision
        rb.freezeRotation = true;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (movement.x > 0)
            transform.localScale = new Vector3(0.14f, 0.165f, 1f);   // face right
        else if (movement.x < 0)
            transform.localScale = new Vector3(-0.14f, 0.165f, 1f);  // face left
    }
}
