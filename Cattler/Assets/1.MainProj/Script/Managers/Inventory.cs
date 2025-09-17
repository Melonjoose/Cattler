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
            int index = inventoryList.IndexOf(item);
            if (index >= 0)
            {
                inventoryList[index] = null; // mark the slot empty
            }
        }
        else if (teamList.Contains(item))
        {
            teamList.Remove(item);
        }
    }

    public bool AddNew(Item item)
    {
        // look for an empty slot (null entry)
        int emptyIndex = inventoryList.FindIndex(i => i == null);
        if (emptyIndex == -1)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        // place the item in the empty slot
        inventoryList[emptyIndex] = item;
        Debug.Log($"Added {item} to inventory slot {emptyIndex}");

        AddItemIntoInventoryUIAt(item, emptyIndex);
        return true;
    }

    void InitializeInventorySpace(int capacity)
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
        }

        // Make sure inventoryList always matches capacity in size
        while (inventoryList.Count < capacity)
            inventoryList.Add(null);

        if (inventoryList.Count > capacity)
            inventoryList.RemoveRange(capacity, inventoryList.Count - capacity);
    }

    void AddItemIntoInventoryUIAt(Item item, int slotIndex)
    {
        GameObject slot = inventorySlots[slotIndex];
        if (slot.transform.childCount > 0)
            Destroy(slot.transform.GetChild(0).gameObject);

        GameObject placeholder = Instantiate(itemPlaceholder, slot.transform); // add ItemPH
        placeholder.transform.localPosition = Vector3.zero;  //center in slot

        ItemUI itemUI = placeholder.GetComponent<ItemUI>();
        itemUI.itemData = item;

        InventoryIcon icon = placeholder.GetComponent<InventoryIcon>();
        icon.catData = item as CatRuntimeData;

        if (itemUI != null)
        {
            // Use the proper type cast
            CatRuntimeData catItem = item as CatRuntimeData;
            itemUI.SetItem(catItem); // this is not working at all.... 
            item.name = catItem.unitName; //name data //working 
            GameObject gameObject = itemUI.gameObject;
            gameObject.name = catItem.unitName; //name GameObject // working

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

            // Keep inventoryList index aligned (add placeholder null)
            if (inventoryList.Count <= i)
                inventoryList.Add(null);
        }

        inventorySlots = slotsList.ToArray();
        currentCapacity = targetCapacity;
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
