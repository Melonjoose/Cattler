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