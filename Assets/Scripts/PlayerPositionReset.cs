using UnityEngine;

[System.Serializable]
public class PlayerAlignment
{
    public Transform playerTransform;
    public Vector2 offsetFromLOS;
}

public class PlayerPositionReset : MonoBehaviour
{
    [Header("LOS Reference")]
    public Transform losMarker;

    [Header("Offensive Players")]
    public PlayerAlignment[] offense;

    [Header("Defensive Players")]
    public PlayerAlignment[] defense;

    public void ResetPlayersRelativeToLOS()
    {
        if (losMarker == null)
        {
            Debug.LogWarning("LOS marker not assigned!");
            return;
        }

        foreach (var p in offense)
        {
            if (p.playerTransform != null)
            {
                p.playerTransform.position = losMarker.position + (Vector3)p.offsetFromLOS;
                p.playerTransform.rotation = Quaternion.identity;
                ResetPhysics(p.playerTransform);
            }
        }

        foreach (var p in defense)
        {
            if (p.playerTransform != null)
            {
                p.playerTransform.position = losMarker.position + (Vector3)p.offsetFromLOS;
                p.playerTransform.rotation = Quaternion.identity;
                ResetPhysics(p.playerTransform);
            }
        }

        Debug.Log("All players reset relative to LOS.");
    }

    private void ResetPhysics(Transform t)
    {
        Rigidbody2D rb = t.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
