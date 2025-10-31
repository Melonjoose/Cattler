using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using static SnappableLocation;
using static UnityEditor.Progress;

public class InventoryIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler , IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;   // renamed to avoid ambiguity
    private CanvasGroup canvasGroup;
    public Transform originalParent;

    public SnappableLocation.ItemType itemType;

    public SnappableLocation originalSlot;
    public SnappableLocation currentSlot;

    [SerializeField] private ItemUI itemUI;
    
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


    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayItemManager.instance.ShowDisplayUI(rectTransform);
        //Debug.Log("MouseHover");
        if (itemUI.itemData != null)
        {
            if(itemUI.itemData is CatData)
            {
                CatUnit catUnit = GetComponent<CatUnit>();
                if (catUnit != null)
                {
                    DisplayItemManager.instance.displayItemUI.Show(this.gameObject);
                }
            }

            if(itemUI.itemData is WeaponData)
            {
                Item weapon = GetComponent<Item>();
                if (weapon != null)
                {
                    DisplayItemManager.instance.displayItemUI.Show(this.gameObject);
                }
            }

            //armor
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayItemManager.instance.HideDisplayUI();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // move to top canvas so it doesn’t get hidden
        canvasGroup.blocksRaycasts = false;

        Inventory.instance.Remove(this.gameObject, currentSlot);
        // Tell slot we are leaving
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
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject dropTarget = eventData.pointerEnter;
        SnappableLocation targetSlot = null;

        if (dropTarget != null)
        {
            targetSlot = dropTarget.GetComponentInParent<SnappableLocation>();
        }

        // If no valid slot or item type mismatch, return to original slot
        if (targetSlot == null || !targetSlot.allowedTypes.Contains(this.itemType))
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
