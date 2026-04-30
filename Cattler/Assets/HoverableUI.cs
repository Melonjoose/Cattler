using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableUI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IEndDragHandler
{ 
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CursorManager.instance.isDragging) { return; }
        CursorManager.instance.SetHoverCursor();        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only reset if not dragging
        if (!eventData.dragging)
            CursorManager.instance.SetDefaultCursor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CursorManager.instance.SetGrabCursor(); // or a dedicated "Click" cursor
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CursorManager.instance.SetDefaultCursor();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CursorManager.instance.SetGrabCursor(); // stays grab until EndDrag
        CursorManager.instance.isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CursorManager.instance.SetDefaultCursor();
        CursorManager.instance.isDragging = false;
    }
}
