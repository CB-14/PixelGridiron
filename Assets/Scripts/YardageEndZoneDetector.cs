using UnityEngine;

public class YardageEndZoneDetector : MonoBehaviour
{
    public float endZoneXLeft = -9.0f;   // Left end zone (Team B scores)
    public float endZoneXRight = 9.0f;   // Right end zone (Team A scores)

    public ScoreboardUI scoreboard;
    public PlayerPositionReset playerReset;

    private bool hasScored = false;

    void Update()
    {
        if (hasScored) return;
         Debug.Log("Ball X Position: " + transform.position.x);

        float x = transform.position.x;

        if (x >= endZoneXRight)
        {
            hasScored = true;

            // ✅ Team A scores
            if (scoreboard != null)
            {
                scoreboard.AddScore(0, 6); // Team A
            }

            ResetPlay();
        }
        else if (x <= endZoneXLeft)
        {
            hasScored = true;

            // ✅ Team B scores
            if (scoreboard != null)
            {
                scoreboard.AddScore(1, 6); // Team B
            }

            ResetPlay();
        }
    }

    private void ResetPlay()
    {
        // Reset player positions
        if (playerReset != null)
        {
            playerReset.ResetPlayersRelativeToLOS();
        }

        // Reset the ball
        BallReset ballReset = GetComponent<BallReset>();
        if (ballReset != null)
        {
            ballReset.ResetBall();
        }

        Debug.Log("Touchdown detected based on X-position!");
    }
}
