using UnityEngine;

[ExecuteInEditMode]
public class PlayerAutoAlignment : MonoBehaviour
{
    public PlayerPositionReset resetManager;

    public void CaptureOffsets()
    {
        if (resetManager == null || resetManager.losMarker == null)
        {
            Debug.LogError("Missing Reset Manager or LOS Marker!");
            return;
        }

        Vector3 losPos = resetManager.losMarker.position;

        // Update offense offsets
        foreach (var player in resetManager.offense)
        {
            if (player.playerTransform != null)
            {
                Vector3 offset = player.playerTransform.position - losPos;
                player.offsetFromLOS = new Vector2(offset.x, offset.y);
            }
        }

        // Update defense offsets
        foreach (var player in resetManager.defense)
        {
            if (player.playerTransform != null)
            {
                Vector3 offset = player.playerTransform.position - losPos;
                player.offsetFromLOS = new Vector2(offset.x, offset.y);
            }
        }

        Debug.Log("Player offsets updated relative to LOS.");
    }
}
