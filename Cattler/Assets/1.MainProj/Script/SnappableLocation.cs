using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public SnappableLocation.SlotType slotSlotType(SnappableLocation slot)
    {
        return slot.slotType;
    }

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
        // If the item was in another slot, tell that slot it’s empty
        if (item.currentSlot != null && item.currentSlot != this)
        {
            item.currentSlot.RemoveItem();
        }

        currentItem = item;
        isOccupied = true;

        // Parent to this slot (UI friendly)
        item.transform.SetParent(transform, false);
        // center in slot
        item.transform.localPosition = Vector3.zero;

        item.SetSlot(this); // tell the icon which slot it is now in
        //Debug.Log("Item placed in slot: " + gameObject.name);

        OnItemPlaced?.Invoke(this);
    }

    public void RemoveItem()
    {
        OnItemRemoved?.Invoke(this);
        isOccupied = false;
        currentItem = null;
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
        RemoveItem();
        if (sourceSlot != null)
            sourceSlot.RemoveItem();

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
