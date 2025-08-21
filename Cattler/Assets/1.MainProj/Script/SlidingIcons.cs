using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SlidingIcons : MonoBehaviour
{
    public List<DraggableIcon> icons = new List<DraggableIcon>();
    public GameObject PositionGRP;
    public List<Transform> iconLocations; // Slots for icons

    private bool isReordering = false;


    /// <summary>
    /// Places icons onto their current slot positions.
    /// </summary> 
    void Start()
    {
        ArrangeIcons();
        AssignIcons(); // make sure cats are synced to positions
        UpdateIconIndex(); // Update indices after assigning icons
    }
    void Update()
    {
        if (!isReordering) return;

        bool allSnapped = true;

        for (int i = 0; i < icons.Count && i < iconLocations.Count; i++)
        {
            if (icons[i] == null) continue;

            // Skip icons being dragged
            DraggableIcon draggable = icons[i].GetComponent<DraggableIcon>();
            if (draggable != null && draggable.isDragging) continue;

            RectTransform iconRect = icons[i].GetComponent<RectTransform>();
            Vector3 targetPos = iconLocations[i].localPosition;

            // Smoothly move towards target
            iconRect.localPosition = Vector3.Lerp(iconRect.localPosition, targetPos, Time.deltaTime * 10f);

            // Check if close enough
            if (Vector3.Distance(iconRect.localPosition, targetPos) > 0.01f)
                allSnapped = false;
        }

        if (allSnapped)
            isReordering = false; // Stop updating when all icons are at their slots
    }

    public void ArrangeIcons() //add all Position1,Position2,etc that is a child of positionGRP to iconLocations
    {
        iconLocations.Clear(); // Clear existing slots
        // Find all child transforms named Position1, Position2, etc.
        foreach (Transform child in PositionGRP.transform)
        {
            if (child.name.StartsWith("Position"))
            {
                iconLocations.Add(child);
            }
        }
        //assign icons to their index located in DraggableIcon.cs

        // Arrange icons in their slots
        SnapIconstoPositions();
    }

    void SnapIconstoPositions()
    {
                for (int i = 0; i < icons.Count; i++)
        {
            if (i < iconLocations.Count)
            {
                icons[i].GetComponent<RectTransform>().localPosition = iconLocations[i].localPosition;
            }
            else
            {
                // If there are more icons than slots, just disable the extra icons
                icons[i].gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// Snap dragged icon to nearest slot & reorder icons list.
    /// </summary>
    public void SnapDraggedIcon(DraggableIcon draggedIcon)
    {
        // Find nearest slot
        float minDist = float.MaxValue;
        int nearestIndex = 0;

        for (int i = 0; i < iconLocations.Count; i++)
        {
            float dist = Vector3.Distance(draggedIcon.GetComponent<RectTransform>().localPosition, iconLocations[i].localPosition);
            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = i;
            }
        }

        // Reorder icons list
        icons.Remove(draggedIcon);
        icons.Insert(nearestIndex, draggedIcon);

        // Snap all icons to positions
        ReorderIcons(draggedIcon);
        UpdateIconIndex(); // Update indices after snapping
        //UpdateCatPositionManager();
    }

    public void ReorderIcons(DraggableIcon draggedIcon)
    {
        isReordering = true; // Start reordering process
        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i] == null || i >= iconLocations.Count) continue;

            RectTransform iconRect = icons[i].GetComponent<RectTransform>();
            Vector3 targetPos = iconLocations[i].localPosition;

            if (icons[i] == draggedIcon)
            {
                // Snap instantly
                iconRect.localPosition = targetPos;
            }
            else
            {
                // Smooth move
                iconRect.localPosition = Vector3.Lerp(iconRect.localPosition, targetPos, 0.5f);
            }
        }
    }

    void AssignIcons()
    {
        icons.Clear();

        foreach (Transform child in transform) // or specific IconGroup
        {
            DraggableIcon draggable = child.GetComponent<DraggableIcon>();
            if (draggable != null)
            {
                icons.Add(draggable);
                draggable.slidingIcons = this; // make sure the reference is set
            }
        }

        SnapIconstoPositions();
    }

    public void UpdateIconIndex()
    {
        // After rearranging icons, reassign indices
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].SetIndex(i);
        }

    }

    public void UpdateCatPositionManager()
    {

    }
}
