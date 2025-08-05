using UnityEngine;

public class OLinemenAI : MonoBehaviour
{
    [Header("Assignments")]
    public Transform assignedDefender; // assign in inspector or at runtime

    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite blockSprite;

    [Header("Blocking Settings")]
    public float blockSpeed = 2f;
    public float bumpForce = 1.2f;
    public float bumpCooldown = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool playStarted = false;
    private float nextBumpTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;

        // ✅ Freeze rotation to prevent spinning
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Wait until hike is triggered
        if (!SnapManager.playStarted) return;

        // On hike, change sprite once
        if (!playStarted)
        {
            playStarted = true;
            sr.sprite = blockSprite;
        }

        if (assignedDefender != null)
        {
            // Actively move toward defender (like a pressure hold)
            Vector2 dir = (assignedDefender.position - transform.position).normalized;
            rb.MovePosition(rb.position + dir * blockSpeed * Time.deltaTime);

            // Bump the defender occasionally
            if (Time.time >= nextBumpTime)
            {
                Rigidbody2D defenderRb = assignedDefender.GetComponent<Rigidbody2D>();
                if (defenderRb != null)
                {
                    defenderRb.AddForce(dir * bumpForce, ForceMode2D.Impulse);
                }
                nextBumpTime = Time.time + bumpCooldown;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Extra bump if defender collides with O-lineman
        if (collision.gameObject.CompareTag("Defense"))
        {
            Rigidbody2D defenderRb = collision.rigidbody;
            if (defenderRb != null)
            {
                Vector2 away = (collision.transform.position - transform.position).normalized;
                defenderRb.AddForce(away * bumpForce * 0.75f, ForceMode2D.Impulse);
            }
        }
    }
}
