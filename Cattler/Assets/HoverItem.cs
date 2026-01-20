using UnityEngine;

public class HoverItem : MonoBehaviour
{
    private HoverHighlightManager manager;

    void Start()
    {
        manager = FindObjectOfType<HoverHighlightManager>();
    }

    void OnMouseEnter()
    {
        if (manager != null)
            manager.OnItemHoverEnter(gameObject);
    }

    void OnMouseExit()
    {
        if (manager != null)
            manager.OnItemHoverExit(gameObject);
    }

    void OnMouseDrag()
    {
        if (manager != null)
            manager.OnItemDragStart(gameObject);
    }

    void OnMouseUp()
    {
        if (manager != null)
            manager.OnItemDragEnd(gameObject);
    }
}