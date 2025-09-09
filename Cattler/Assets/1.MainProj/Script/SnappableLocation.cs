using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class SnappableLocation : MonoBehaviour, IDropHandler
{
    public bool isOccupied { get; private set; }
    
    public DraggableIcon currentItem { get; private set; }
    
    public event Action<SnappableLocation> OnItemRemoved;
    public void ClearItem()
    {
        currentItem = null;
        isOccupied = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isOccupied) return; // slot already full
        Debug.Log("OnDrop triggered on " + gameObject.name);
        DraggableIcon droppedItem = eventData.pointerDrag.GetComponent<DraggableIcon>();
        if (droppedItem != null)
        {
            PlaceItem(droppedItem);
        }
    }

    public void PlaceItem(DraggableIcon item)
    {
        currentItem = item;
        isOccupied = true;
        item.transform.SetParent(transform, false); // snap
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

