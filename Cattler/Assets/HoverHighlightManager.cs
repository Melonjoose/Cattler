using UnityEngine;

public class HoverHighlightManager : MonoBehaviour
{
    public GameObject highlighter;       // The shared arrow indicator
    public Vector2 offset;               // Offset for arrow position

    private GameObject currentItem;      // The item currently hovered
    public bool isDragging = false;      // Is an item being dragged

    void Start()
    {
        if (highlighter == null)
            highlighter = GameObject.Find("ArrowIndicator");

        if (highlighter != null)
            highlighter.SetActive(false);
    }

    void Update()
    {
        // Continuously follow the item if arrow is active and not dragging
        if (!isDragging && highlighter != null && highlighter.activeSelf && currentItem != null)
        {
            UpdateHighlighterPosition();
        }
    }

    // Called when mouse enters an item collider
    public void OnItemHoverEnter(GameObject item)
    {
        if (!isDragging && highlighter != null)
        {
            currentItem = item;
            highlighter.SetActive(true);
            UpdateHighlighterPosition();
        }
    }

    // Called when mouse exits an item collider
    public void OnItemHoverExit(GameObject item)
    {
        if (!isDragging && currentItem == item && highlighter != null)
        {
            currentItem = null;
            highlighter.SetActive(false);
        }
    }

    // Called when dragging starts on an item
    public void OnItemDragStart(GameObject item)
    {
        if (currentItem == item && highlighter != null)
        {
            isDragging = true;
            highlighter.SetActive(false);
        }
    }

    // Called when dragging ends on an item
    public void OnItemDragEnd(GameObject item)
    {
        if (currentItem == item)
        {
            isDragging = false;
            // Optionally reenable the arrow if still hovered
            if (highlighter != null)
            {
                highlighter.SetActive(true);
                UpdateHighlighterPosition();
            }
        }
    }

    private void UpdateHighlighterPosition()
    {
        if (currentItem != null && highlighter != null)
        {
            highlighter.transform.position = new Vector2(
                currentItem.transform.position.x + offset.x,
                currentItem.transform.position.y + offset.y
            );
        }
    }
}