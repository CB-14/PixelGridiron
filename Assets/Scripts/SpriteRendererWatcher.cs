using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererWatcher : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool lastEnabledState;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastEnabledState = spriteRenderer.enabled;
    }

    void Update()
    {
        if (spriteRenderer.enabled != lastEnabledState)
        {
            Debug.LogWarning($"[{Time.time:F2}s] SpriteRenderer enabled state changed on '{gameObject.name}' from {lastEnabledState} to {spriteRenderer.enabled}");
            lastEnabledState = spriteRenderer.enabled;
        }
    }
}
