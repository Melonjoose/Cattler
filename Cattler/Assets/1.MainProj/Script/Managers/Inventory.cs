using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private bool isInitialized = false;
    public List<Item> items = new List<Item>();

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
    }

    public bool Add(Item item)
    {
        if (items.Count >= currentCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        items.Add(item);
        Debug.Log($"Added {item} to inventory.");

        AddItemIntoInventoryUI(item);

        return true;
    }

    public void Remove(Item item)
    {
        int index = items.IndexOf(item);
        if (index >= 0)
        {
            items.RemoveAt(index);

            // Remove UI placeholder
            Transform slot = inventorySlots[index].transform;
            if (slot.childCount > 0)
                Destroy(slot.GetChild(0).gameObject);
        }
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
        if (items.Count > capacity)
            items.RemoveRange(capacity, items.Count - capacity);
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

                // Assign the data to the placeholder
                ItemUI itemUI = placeholder.GetComponent<ItemUI>();
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

}
