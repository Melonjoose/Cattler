using UnityEngine;

public class HoverItem : MonoBehaviour
{
    private HoverHighlightManager manager;
    public Vector3 offset = new Vector3(0, 1, 0);
    
    void Start()
    {
        manager = HoverHighlightManager.instance;
        //hoverStartPoint = transform.position + offset;
    }
    void Update()
    {

    }

    void OnMouseEnter()
    {
        if (manager != null)
        {
            manager.OnItemHoverEnter(gameObject, offset);
        }
        if (!CursorManager.instance.isDragging)
        {
            CursorManager.instance.SetHoverCursor();
        }
    }

    void OnMouseExit()
    {
        if (manager != null)
        {
            manager.OnItemHoverExit(gameObject, offset);
        }
        if (!CursorManager.instance.isDragging)
        {
            CursorManager.instance.SetDefaultCursor();
        }
    }

    private void OnMouseDown()
    {
        CursorManager.instance.SetGrabCursor();
        CursorManager.instance.isDragging = true;
    }

    void OnMouseDrag()
    {
        if (manager != null)
        {
            manager.OnItemDragStart(gameObject, offset);
        }
        CursorManager.instance.SetGrabCursor();
    }

    void OnMouseUp()
    {
        if (manager != null)
            manager.OnItemDragEnd(gameObject, offset);
        CursorManager.instance.SetDefaultCursor();
        CursorManager.instance.isDragging = false;
    }
}