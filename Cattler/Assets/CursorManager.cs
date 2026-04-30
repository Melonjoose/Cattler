using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D cursorDefault;
    public Texture2D cursorHover;
    public Texture2D cursorGrab;

    private Vector2 hotspot = Vector2.zero; // adjust if you want center hotspot

    
    public bool isDragging = false;
    void Start()
    {
        instance = this;
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorDefault, hotspot, CursorMode.Auto);
    }

    public void SetHoverCursor()
    {
        Cursor.SetCursor(cursorHover, hotspot, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(cursorGrab, hotspot, CursorMode.Auto);
    }
}
