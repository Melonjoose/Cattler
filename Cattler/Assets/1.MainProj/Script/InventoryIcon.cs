using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UIElements;
using System.Data;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler , IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;   // renamed to avoid ambiguity
    public CanvasGroup canvasGroup;
    public Transform originalParent;

    public SnappableLocation.ItemType itemType;

    public SnappableLocation originalSlot;
    public SnappableLocation currentSlot;

    public bool isDragging = false;

    [SerializeField] private ItemUI itemUI;
    public InventoryIcon selectedObject;

    private float lastRightClickTime = 0f;
    private float doubleClickThreshold = 0.3f; // seconds between clicks

    private void Awake()
    {
        itemUI = GetComponent<ItemUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Start()
    {
        originalParent = transform.parent;
    }
    private void Update()
    {
        // Only act if THIS icon is the selected one
        if (selectedObject == this && Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastRightClickTime < doubleClickThreshold)
            {
                // Double right click detected on this specific icon
                Inventory.instance.Remove(this.gameObject, currentSlot);
                RemoveItemFromSlot();
                DisplayItemManager.instance.HideDisplayUI();
                Destroy(this.gameObject);
                AudioManager.instance.PlaySFX("Trash");
            }
            lastRightClickTime = Time.time;
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("MouseHover");
        if (itemUI.itemData != null)
        {
            if(itemUI.itemData is CatData)
            {
                CatUnit catUnit = GetComponent<CatUnit>();
                if (catUnit != null)
                {
                    DisplayItemManager.instance.ShowDisplayUI(this.gameObject);
                }
            }

            if(itemUI.itemData is WeaponData)
            {
                Item weapon = GetComponent<Item>();
                if (weapon != null)
                {
                    DisplayItemManager.instance.ShowDisplayUI(this.gameObject);
                }
            }

            selectedObject = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedObject = null;
        DisplayItemManager.instance.HideDisplayUI();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Normal drag logic
        if (isDragging) return;
        if (currentSlot == null) currentSlot = originalSlot;

        isDragging = true;
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;

        Inventory.instance.Remove(this.gameObject, currentSlot);
        RemoveItemFromSlot();
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        if (parentCanvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false; // force reset

        //canvasGroup.alpha = 1f;  //should enable only if they reach location.
        //canvasGroup.blocksRaycasts = true; //should enable only if they reach location.

        GameObject dropTarget = eventData.pointerEnter;
        SnappableLocation targetSlot = null;

        if (dropTarget != null)
        {
            targetSlot = dropTarget.GetComponentInParent<SnappableLocation>();
        }

        // If no valid slot or item type mismatch, return to original slot
        if (targetSlot == null || !targetSlot.allowedTypes.Contains(this.itemType) || targetSlot.blockSnapping)
        {
            currentSlot = originalSlot;
            Inventory.instance.PlaceItem(this, originalSlot);
            return;
        }

        // If slot is empty, place item
        if (targetSlot.currentItem == null)
        {
            Inventory.instance.PlaceItem(this, targetSlot);
        }
        else
        {
            // Slot occupied: swap items
            Inventory.instance.SwapItem(this, originalSlot, targetSlot);
        }
    }

    public void RemoveItemFromSlot()
    {
        if (currentSlot != null)
        {
            originalSlot = currentSlot;
            currentSlot.isOccupied = false;
            currentSlot.currentItem = null;
            currentSlot = null;
        }
    }
}
