using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Texture to use for the cursor when hovering over the UI element.")]
    public Texture2D cursorTexture;

    [Tooltip("Cursor hotspot (offset from top-left)")]
    public Vector2 hotSpot = Vector2.zero;

    // Called when the mouse pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    // Called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Resets to default cursor
    }
}
