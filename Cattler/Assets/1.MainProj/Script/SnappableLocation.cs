using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class SnappableLocation : MonoBehaviour, IDropHandler
{
    public bool isOccupied;
    public InventoryIcon currentItem;
    public int SlotIndex = 0;

    [Header("Allowed Item Types")]
    public ItemType[] allowedTypes; // set in Inspector
    public enum ItemType
    {
        Cat,
        Weapon,
        Hat
    }

    public enum SlotType
    {
        InventoryList,
        TeamList,
        CharacterPreview
    }

    public SlotType slotType; // set in inspector per slot

    // Events
    public event Action<SnappableLocation> OnItemPlaced;
    public event Action<SnappableLocation> OnItemRemoved;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryIcon droppedItem = eventData.pointerDrag?.GetComponent<InventoryIcon>();
        if (droppedItem == null) return;

        Debug.Log($"{gameObject.name} isOccupied = {isOccupied}");

        if (!IsAllowed(droppedItem.itemType))
        {
            Debug.Log($"Item type {droppedItem.itemType} not allowed in slot {gameObject.name}");
            return;
        }

        if (isOccupied)
        {
            SwapItem(droppedItem);
        }
        else
        {
            PlaceItem(droppedItem);
        }


    }

    /// <summary>
    /// Place an item into this slot. Handles removing the item from its old slot (if any).
    /// </summary>
    public void PlaceItem(InventoryIcon item)
    {
        if (item.currentSlot != null && item.currentSlot != this)
        {
            item.currentSlot.RemoveItem(item);

        }

        currentItem = item;
        isOccupied = true;

        // Store original world position
        Vector3 startPos = item.transform.position;
        Vector3 endPos = transform.position;
        Vector3 targetScale = transform.localScale;

        // Temporarily keep world position before parenting
        //item.transform.SetParent(transform.parent, true);
        Transform tweenParent = Inventory.instance.transform;
        item.transform.SetParent(transform.parent, true);

        // Animate both movement and scale together
        float tweenDuration = 0.3f;

        LeanTween.move(item.gameObject, endPos, tweenDuration)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.scale(item.gameObject, targetScale, tweenDuration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                // After animation finishes, parent to slot
                item.transform.SetParent(transform, false);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = targetScale;
            });

        item.SetSlot(this);
        
        OnItemPlaced?.Invoke(this); // Notify listeners

        if (this.CompareTag("CatPreviewSlot")) { SelectedItemDisplayUI.instance.ShowCatStats(this); }
        Inventory.instance.Add(item.gameObject, this);
    }


    public void RemoveItem(InventoryIcon item)
    {
        isOccupied = false;

        Inventory.instance.Remove(item.gameObject, this); // pass real slot

        currentItem = null;
        OnItemRemoved?.Invoke(this);
        if (this.CompareTag("CatPreviewSlot")) { SelectedItemDisplayUI.instance.ShowCatStats(this); }
        
    }

    /// <summary>
    /// Swap the dragged item (newItem) with the current occupant of this slot.
    /// newItem.originalSlot should point to where the dragged item came from.
    /// </summary>
    private void SwapItem(InventoryIcon draggedItem)
    {
        if (draggedItem == null || currentItem == null) return;

        InventoryIcon oldItem = currentItem;                // item currently in this slot
        SnappableLocation sourceSlot = draggedItem.originalSlot; // where dragged item came from

        // Step 1: remove both from their slots temporarily

        RemoveItem(draggedItem);

        if (sourceSlot != null)
            sourceSlot.RemoveItem(oldItem);

        // Step 2: place dragged item into this slot
        PlaceItem(draggedItem);

        // Step 3: place old item into dragged item's original slot (or fallback)
        if (sourceSlot != null)
        {
            sourceSlot.PlaceItem(oldItem);
            
        }
        else
        {
            // fallback: parent to original transform stored in InventoryIcon
            oldItem.transform.SetParent(oldItem.originalParent, false);
            oldItem.transform.localPosition = Vector3.zero;
            oldItem.SetSlot(null);
           
        }
        isOccupied = true;
        currentItem = draggedItem;

        if (this.CompareTag("CatPreviewSlot")) { SelectedItemDisplayUI.instance.ShowCatStats(this); }
    }



    private bool IsAllowed(ItemType type)
    {
        foreach (var allowed in allowedTypes)
        {
            if (allowed == type) return true;
        }
        return false;
    }

}
