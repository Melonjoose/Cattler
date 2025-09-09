using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SnappableLocation : MonoBehaviour, IDropHandler
{
    public bool isOccupied { get; private set; }
    public InventoryIcon currentItem { get; private set; }

    public event Action<SnappableLocation> OnItemRemoved;

    public void OnDrop(PointerEventData eventData)
    {
        if (isOccupied) return; // slot already full
        Debug.Log("OnDrop triggered on " + gameObject.name);

        InventoryIcon droppedItem = eventData.pointerDrag.GetComponent<InventoryIcon>();
        if (droppedItem != null)
        {
            PlaceItem(droppedItem);
        }
    }

    public void PlaceItem(InventoryIcon item)
    {
        currentItem = item;
        isOccupied = true;
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero;
        Debug.Log("Item placed in slot: " + gameObject.name);
    }

    public void RemoveItem()
    {
        OnItemRemoved?.Invoke(this);
        isOccupied = false;
        currentItem = null;
    }
}
