using UnityEngine;
using UnityEngine.EventSystems;
using static SnappableLocation;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;   // renamed to avoid ambiguity
    private CanvasGroup canvasGroup;
    public Transform originalParent;

    public SnappableLocation.ItemType itemType;

    public SnappableLocation originalSlot;
    public SnappableLocation currentSlot;

    public CatRuntimeData catData; // reference to the cat data this icon represents

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Start()
    {
        // Snap to nearest slot at start
        SnapToNearestSlot();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // move to top canvas so it doesn’t get hidden
        canvasGroup.blocksRaycasts = false;

        // Tell slot we are leaving
        if (currentSlot != null)
        {
            originalSlot = currentSlot; // remember slot for swap
            currentSlot.RemoveItem();
            currentSlot = null;
        }
        Inventory.instance.Remove(catData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentCanvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // if dropped on empty space, return to original slot
        if (transform.parent == transform.root && originalParent != null)
        {
            currentSlot = originalSlot;
            transform.SetParent(originalParent, false);
            transform.localPosition = Vector3.zero;
            currentSlot?.PlaceItem(this);
        }

        Inventory.instance.Add(catData);
    }

    public void SetSlot(SnappableLocation slot)
    {
        currentSlot = slot;
    }

    public void SnapToNearestSlot()
    {
        SnappableLocation[] slots = FindObjectsOfType<SnappableLocation>();

        SnappableLocation nearestSlot = null;
        float nearestDistance = float.MaxValue;

        foreach (var slot in slots)
        {
            // Skip occupied slots unless it's the one we're already in
            if (slot.isOccupied && slot != currentSlot) continue;

            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestSlot = slot;
            }
        }

        if (nearestSlot != null)
        {
            // Snap into slot
            nearestSlot.PlaceItem(this);
            currentSlot = nearestSlot;
            originalSlot = nearestSlot;
        }
        else
        {
            Debug.LogWarning("No available slot found to snap into!");
        }

        nearestSlot.isOccupied = true; // Mark the slot as occupied
        nearestSlot.currentItem = this; // Set the current item in the slot
    }
}
