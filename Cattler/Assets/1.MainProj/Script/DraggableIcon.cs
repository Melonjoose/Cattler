using UnityEngine;
using UnityEngine.EventSystems; // Needed for drag/drop interfaces

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // The UI object we are dragging
    private Canvas canvas; // Needed to calculate proper position
    private CanvasGroup canvasGroup; // Helps with raycasting (interaction)

    public SlidingIcons slidingIcons; // reference to manager
    public bool isDragging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Find the parent canvas (needed for correct mouse position conversion)
        canvas = GetComponentInParent<Canvas>();

    }

    // Called when drag begins (mouse button pressed down on this object)
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        // Make the icon semi-transparent while dragging
        canvasGroup.alpha = 0.6f;

        // Disable raycast so we don't block other UI elements while dragging
        canvasGroup.blocksRaycasts = false;
    }

    // Called every frame while dragging
    public void OnDrag(PointerEventData eventData)
    {
        // Move the icon with the mouse
        // eventData.delta = mouse movement in pixels
        // We use canvas scale to adjust movement properly
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // Called when drag ends (mouse released)
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Snap to nearest slot
        if (slidingIcons != null)
        {
            slidingIcons.SnapDraggedIcon(this);
        }
    }
}
