using UnityEngine;

public class HoverHighlightManager : MonoBehaviour
{
    public static HoverHighlightManager instance; 
    public GameObject highlighter;       // The shared arrow indicator
    public Vector3 managerOffset;

    private GameObject currentItem;      // The item currently hovered
    public bool isDragging = false;      // Is an item being dragged

    void Start()
    {
        instance = this;

        if (highlighter == null)
            highlighter = GameObject.Find("ArrowIndicator");

        if (highlighter != null)
            highlighter.SetActive(false);
    }

    void Update()
    {
        if (isDragging && currentItem == null)
        {
            isDragging = false;
            highlighter?.SetActive(false);
        }

        if (!isDragging && highlighter != null && highlighter.activeSelf && currentItem != null)
        {
            // Just keep following currentItem without offset
            UpdateHighlighterPosition(managerOffset);
        }
    }


    // Called when mouse enters an item collider
    public void OnItemHoverEnter(GameObject item , Vector3 offset)
    {
        if (!isDragging && highlighter != null)
        {
            managerOffset = offset; // Update the offset for this item
            currentItem = item;
            highlighter.SetActive(true);
            UpdateHighlighterPosition(offset);
        }
    }

    // Called when mouse exits an item collider
    public void OnItemHoverExit(GameObject item, Vector3 offset)
    {
        if (!isDragging && currentItem == item && highlighter != null)
        {
            currentItem = null;
            highlighter.SetActive(false);
        }
    }

    // Called when dragging starts on an item
    public void OnItemDragStart(GameObject item, Vector3 offset)
    {
        if (currentItem == item && highlighter != null)
        {
            isDragging = true;
            highlighter.SetActive(false);
        }
    }

    // Called when dragging ends on an item
    public void OnItemDragEnd(GameObject item, Vector3 offset)
    {
        if (currentItem == item)
        {
            isDragging = false;
            // Optionally reenable the arrow if still hovered
            if (highlighter != null)
            {
                highlighter.SetActive(true);
                UpdateHighlighterPosition(offset);
            }
        }
    }
    private void UpdateHighlighterPosition(Vector3 offset)
    {
        if (currentItem != null && highlighter != null)
        {
            highlighter.transform.position = currentItem.transform.position + offset;
        }
    }
}