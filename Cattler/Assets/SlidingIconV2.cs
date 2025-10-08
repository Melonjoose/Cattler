using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingIconV2 : MonoBehaviour
{
    public GameObject iconGRP;
    public List<Icon> icons = new List<Icon>(); // Array to hold references to CatIcons
    public GameObject PositionGRP;
    public List<Transform> iconLocations = new List<Transform>(); // Slots for icons
    public List<Transform> catPositions = new List<Transform>(); // world positions for cats

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArrangeIcons();      // Build slot list
        AssignIcons();       // Find draggable icons and register them
        UpdateIconIndex();   // Sync indices immediately
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //if CatUnit.index is 0, move catIcons[0] to iconPositions[0]

    //if 
    void GetCatIndex(int catIndex)
    {

               // Sync icon index with cat position index
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].AssignIconIndex(catIndex);
        }
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

        foreach (Icon icon in iconGRP.transform)
        {
            if (icon.name.StartsWith("Position"))
                icons.Add(icon);
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
            {
                icons[i].GetComponent<RectTransform>().localPosition = iconLocations[i].localPosition;
            }

            else
            {
                icons[i].gameObject.SetActive(false); // too many icons for slots
            }

        }
    }

    public void SnapDraggedIcon(Icon draggedIcon)
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
    }

    public void ReorderIcons(Icon draggedIcon)
    {
        //isReordering = true;

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
            icons[i].AssignIconIndex(i);
        }
    }


}
