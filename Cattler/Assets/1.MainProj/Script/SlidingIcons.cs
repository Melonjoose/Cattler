using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlidingIcons : MonoBehaviour
{
    public List<DraggableIcon> icons = new List<DraggableIcon>();
    public GameObject PositionGRP;
    public List<Transform> iconLocations = new List<Transform>(); // Slots for icons
    public List<Transform> catPositions = new List<Transform>(); // world positions for cats

    private bool isReordering = false;

    private int index;
    private CatMovement assignedCat;
    void Start()
    {
        ArrangeIcons();      // Build slot list
        AssignIcons();       // Find draggable icons and register them
        UpdateIconIndex();   // Sync indices immediately
        //CatPositionManager.instance.UpdateIconIndices(); // Sync with CatPositionManager

    }

    void Update()
    {
        if (!isReordering) return;

        bool allSnapped = true;

        for (int i = 0; i < icons.Count && i < iconLocations.Count; i++)
        {
            if (icons[i] == null) continue;

            // Skip icons being dragged
            DraggableIcon draggable = icons[i];
            if (draggable != null && draggable.isDragging) continue;

            RectTransform iconRect = icons[i].GetComponent<RectTransform>();
            Vector3 targetPos = iconLocations[i].localPosition;

            // Smoothly move towards target
            iconRect.localPosition = Vector3.Lerp(iconRect.localPosition, targetPos, Time.deltaTime * 10f);

            if (Vector3.Distance(iconRect.localPosition, targetPos) > 0.01f)
                allSnapped = false;
        }

        if (allSnapped)
            isReordering = false; // Stop updating when all icons are at their slots
    }

    // ---------------------------
    // Setup
    // ---------------------------
    public void ArrangeIcons()
    {
        iconLocations.Clear();

        foreach (Transform child in PositionGRP.transform)
        {
            if (child.name.StartsWith("Position"))
                iconLocations.Add(child);
        }

        SnapIconsToPositions();
        UpdateIconIndex(); // make sure indices are synced
    }

    void AssignIcons()
    {
        icons.Clear();

        foreach (Transform child in transform) // or a dedicated IconGroup
        {
            DraggableIcon draggable = child.GetComponent<DraggableIcon>();
            if (draggable != null)
            {
                icons.Add(draggable);
                draggable.slidingIcons = this;
            }
        }

        SnapIconsToPositions();
        UpdateIconIndex(); // sync indices
    }

    // ---------------------------
    // Icon Positioning
    // ---------------------------
    void SnapIconsToPositions() // Force Icons to their slots
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (i < iconLocations.Count)
                icons[i].GetComponent<RectTransform>().localPosition = iconLocations[i].localPosition;
            else
                icons[i].gameObject.SetActive(false); // too many icons for slots
        }
    }

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

        // Reorder list
        icons.Remove(draggedIcon);
        icons.Insert(nearestIndex, draggedIcon);

        // Rearrange
        ReorderIcons(draggedIcon);
        UpdateIconIndex(); // <--- important: sync indices here
        CatPositionManager.instance.UpdateManager(); // Sync with CatPositionManager
    }

    public void ReorderIcons(DraggableIcon draggedIcon)
    {
        isReordering = true;

        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i] == null || i >= iconLocations.Count) continue;

            RectTransform iconRect = icons[i].GetComponent<RectTransform>();
            Vector3 targetPos = iconLocations[i].localPosition;

            if (icons[i] == draggedIcon)
                iconRect.localPosition = targetPos; // snap instantly
            else
                iconRect.localPosition = Vector3.Lerp(iconRect.localPosition, targetPos, 0.5f);
        }

    }

    // ---------------------------
    // Sync
    // ---------------------------
    public void UpdateIconIndex()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].SetIndex(i);
        }
    }

}
