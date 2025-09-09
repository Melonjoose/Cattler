using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private SnappableLocation currentSlot;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        currentSlot = originalParent.GetComponent<SnappableLocation>();

        if (currentSlot != null)
        {
            currentSlot.RemoveItem(); // notify slot
        }

        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        canvasGroup.alpha = 0.6f;          // Semi-transparent
        canvasGroup.blocksRaycasts = false; // Allow drop targets to receive raycasts
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // if dropped on empty space, return to original slot
        if (transform.parent == transform.root && originalParent != null)
        {
            transform.SetParent(originalParent, false);
            transform.localPosition = Vector3.zero;
            currentSlot?.PlaceItem(this);
        }
    }
}
