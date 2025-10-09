using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<GameObject> inventoryList = new List<GameObject>();
    public List<GameObject> teamList = new List<GameObject>();
    public List<GameObject> previewList = new List<GameObject>();

    public int currentCapacity = 5;
    public int maxCapacity = 60;

    [Header("UI")]
    public ItemData testItem;
    
    public GameObject inventoryGRP;          // Parent object for inventory slots
    public GameObject inventorySlotPrefab;   // Prefab for slot
    public GameObject itemPlaceholder;       // Prefab for item icons in UI

    private GameObject[] inventorySlots;
    public SnappableLocation[] teamSlots;
    public SnappableLocation[] previewSlots;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        InitializeInventorySpace(currentCapacity);

        foreach (GameObject slotObj in inventorySlots)
        {
            SnappableLocation slot = slotObj.GetComponent<SnappableLocation>();
            slot.OnItemPlaced += OnItemPlaced;
            slot.OnItemRemoved += OnItemRemoved;
        }
        foreach (SnappableLocation slotObj in teamSlots)
        {
            SnappableLocation slot = slotObj.GetComponent<SnappableLocation>();
            slot.OnItemPlaced += OnItemPlaced;
            slot.OnItemRemoved += OnItemRemoved;
        }
        foreach (SnappableLocation slotObj in previewSlots)
        {
            SnappableLocation slot = slotObj.GetComponent<SnappableLocation>();
            slot.OnItemPlaced += OnItemPlaced;
            slot.OnItemRemoved += OnItemRemoved;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            InstantiateNew(testItem);
        }
    }

    public void Add(GameObject Item , SnappableLocation slot)  //when added new or when moving items around
    {
        if(slot.slotType == SnappableLocation.SlotType.InventoryList)
        {
            inventoryList.Add(Item);
            //Debug.Log($"Added {Item} to inventory.");
        }
        else if(slot.slotType == SnappableLocation.SlotType.TeamList)
        {
            teamList.Add(Item);
            //Debug.Log($"Added {Item} to Teamlist.");
        }
        else if(slot.slotType == SnappableLocation.SlotType.CharacterPreview)
        {
            previewList.Add(Item);
            //Debug.Log($"Added {Item} to CharacterPreview.");
        }
    }

    public void InstantiateNew(ItemData itemData)
    {
        if (inventoryList.Count >= currentCapacity) { Debug.LogWarning("Inventory full"); return; }
        SnappableLocation emptySlot = GetFirstEmptySlot();
        GameObject prefab = Instantiate(itemPlaceholder , emptySlot.transform);
        ItemUI newItem = prefab.GetComponent<ItemUI>();
        if (newItem != null)
        {
            prefab.name = itemData.itemName;

            newItem.itemData = itemData;

            newItem.iconImage.sprite = itemData.icon;
        }
    }

    public void Remove(GameObject Item, SnappableLocation slot)
    {
        inventoryList.Remove(Item);
        teamList.Remove(Item);
        previewList.Remove(Item);
    }

    SnappableLocation GetFirstEmptySlot()
    {
        foreach (GameObject slotObj in inventorySlots)
        {
            SnappableLocation slot = slotObj.GetComponent<SnappableLocation>();

            if (slot != null && !slot.isOccupied)
            {
                return slot;
            }
        }
        return null;
    }

    public void AddToSlotAsChild(GameObject item, int slotIndex) //when moving items around
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Length)
        {
            Debug.LogError("Invalid slot index");
            return;
        }
        item.transform.SetParent(inventorySlots[slotIndex].transform);
        item.transform.localPosition = Vector3.zero; // Center in slot
        item.transform.localScale = Vector3.one;    // Reset scale
    }


    void InitializeInventorySpace(int capacity) //Adding UI slots and assign slotindex.
    {
        if (capacity > maxCapacity) capacity = maxCapacity;
        currentCapacity = capacity;

        // Destroy old slots (UI only)
        foreach (Transform child in inventoryGRP.transform)
            Destroy(child.gameObject);

        // Recreate slots
        inventorySlots = new GameObject[capacity];
        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform);
            slot.name = $"InventorySlot{i + 1}";
            inventorySlots[i] = slot;
            SnappableLocation Slot = slot.GetComponent<SnappableLocation>();
            Slot.SlotIndex = i;
        }

    }

    public void IncreaseCapacity(int addedSlots)
    {
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log("Inventory at max capacity!");
            return;
        }

        int targetCapacity = Mathf.Min(currentCapacity + addedSlots, maxCapacity);

        // Expand inventorySlots array safely
        List<GameObject> slotsList = new List<GameObject>(inventorySlots);

        for (int i = currentCapacity; i < targetCapacity; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform); //add UI slots
            slot.name = $"InventorySlot{i + 1}";
            slotsList.Add(slot);
        }

        inventorySlots = slotsList.ToArray();
        currentCapacity = targetCapacity;
    }

    private void OnItemPlaced(SnappableLocation slot)
    {
        Debug.Log($"Item placed in slot {slot.name}");
        Debug.Log($"Slot index is {slot.SlotIndex}");
        if (slot.slotType == SnappableLocation.SlotType.TeamList) 
        {

        }

    }

    private void OnItemRemoved(SnappableLocation slot)
    {
        Debug.Log($"Item removed from slot {slot.name}");
    }
}
