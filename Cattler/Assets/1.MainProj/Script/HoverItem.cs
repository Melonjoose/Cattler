using UnityEngine;

public class HoverItem : MonoBehaviour
{
    private HoverHighlightManager manager;
    public Vector3 offset;

    void Start()
    {
        manager = FindObjectOfType<HoverHighlightManager>();
        //hoverStartPoint = transform.position + offset;
    }

    void OnMouseEnter()
    {
        if (manager != null)
            manager.OnItemHoverEnter(gameObject, offset);
    }

    void OnMouseExit()
    {
        if (manager != null)
            manager.OnItemHoverExit(gameObject, offset);
    }

    void OnMouseDrag()
    {
        if (manager != null)
            manager.OnItemDragStart(gameObject, offset);
    }

    void OnMouseUp()
    {
        if (manager != null)
            manager.OnItemDragEnd(gameObject, offset);
    }
}