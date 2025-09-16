using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private bool isInitialized = false;
    public List<Item> inventoryList = new List<Item>();
    public List<Item> teamList = new List<Item>();

    public int currentCapacity = 5;
    public int maxCapacity = 60;

    [Header("UI")]
    public GameObject inventoryGRP;          // Parent object for inventory slots
    public GameObject inventorySlotPrefab;   // Prefab for slot
    public GameObject itemPlaceholder;       // Prefab for item icons in UI

    private GameObject[] inventorySlots;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        InitializeInventorySpace(currentCapacity);

        // Subscribe to all slots
        foreach (var slot in FindObjectsOfType<SnappableLocation>())
        {
            slot.OnItemPlaced += HandleItemPlaced;
            slot.OnItemRemoved += HandleItemRemoved;
        }
    }

    public bool Add(Item item)
    {
        if (inventoryList.Count >= currentCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        Debug.Log($"Added {item} to inventory.");

        return true;
    }

    public void Remove(Item item)
    {
        if (inventoryList.Contains(item))
        {
            inventoryList.Remove(item);
        }
        else if (teamList.Contains(item))
        {
            teamList.Remove(item);
        }
    }

    public bool AddNew(Item item)
    {
        if (inventoryList.Count >= currentCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        
        Debug.Log($"Added {item} to inventory.");

        AddItemIntoInventoryUI(item);

        return true;
    }

    void InitializeInventorySpace(int capacity)
    {
        if (capacity > maxCapacity) capacity = maxCapacity;
        currentCapacity = capacity;

        // Destroy old slots (run once at start)
        foreach (Transform child in inventoryGRP.transform)
            Destroy(child.gameObject);

        // Recreate slots
        inventorySlots = new GameObject[capacity];
        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform);
            slot.name = $"InventorySlot{i + 1}"; // rename sequentially
            inventorySlots[i] = slot;
        }

        // Trim items if needed
        if (inventoryList.Count > capacity)
            inventoryList.RemoveRange(capacity, inventoryList.Count - capacity);
    }

    public void IncreaseCapacity(int addedSlots)
    {
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log("Inventory at max capacity!");
            return;
        }

        for (int i = 0; i < addedSlots; i++)
        {
            if (currentCapacity >= maxCapacity) break;

            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform);
            slot.name = $"InventorySlot{currentCapacity + 1}"; // continue sequence
                                                               // Optionally, add to inventorySlots array if you maintain it dynamically
            currentCapacity++;
        }
    }

    //  Link item data to UI
    void AddItemIntoInventoryUI(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount == 0)
            {
                // Instantiate placeholder in the empty slot
                GameObject placeholder = Instantiate(itemPlaceholder, inventorySlots[i].transform);
                placeholder.transform.localPosition = Vector3.zero;
           
                ItemUI itemUI = placeholder.GetComponent<ItemUI>();
                itemUI.itemData = item;
                InventoryIcon icon = placeholder.GetComponent<InventoryIcon>();
                icon.catData = item as CatRuntimeData;
                if (itemUI != null)
                {
                    // Use the proper type cast
                    CatRuntimeData catItem = item as CatRuntimeData;
                    if (catItem != null)
                        itemUI.SetItem(catItem);
                }

                break; // stop after filling one slot
            }
        }
    }



    void RemoveFromList(Item item)
    {
        int index = inventoryList.IndexOf(item);
        if (index >= 0)
        {
            inventoryList.RemoveAt(index);

            // Remove UI placeholder
            Transform slot = inventorySlots[index].transform;
            if (slot.childCount > 0)
                Destroy(slot.GetChild(0).gameObject);
        }
    }

    private void HandleItemPlaced(SnappableLocation slot)
    {
        if (slot.currentItem == null) return;

        Item item = slot.currentItem.catData;

        switch (slot.slotType)
        {
            case SnappableLocation.SlotType.InventoryList:
                if (!inventoryList.Contains(item))
                    inventoryList.Add(item);
                break;

            case SnappableLocation.SlotType.TeamList:
                if (!teamList.Contains(item))
                    teamList.Add(item);
                break;

            case SnappableLocation.SlotType.CharacterPreview:
                // optional: preview slot doesn’t store in lists
                break;
        }
    }

    private void HandleItemRemoved(SnappableLocation slot)
    {
        if (slot.currentItem == null) return; // careful: after removal, currentItem may already be null
        Item item = slot.currentItem.catData;

        switch (slot.slotType)
        {
            case SnappableLocation.SlotType.InventoryList:
                inventoryList.Remove(item);
                break;

            case SnappableLocation.SlotType.TeamList:
                teamList.Remove(item);
                break;
        }
    }




}
