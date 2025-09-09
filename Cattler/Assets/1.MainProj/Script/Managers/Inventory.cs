using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
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

        SetInventorySpace(currentCapacity);
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

        AddIntoInventoryUI(item);
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

    void SetInventorySpace(int capacity)
    {
        if (capacity > maxCapacity) capacity = maxCapacity;
        currentCapacity = capacity;

        // Destroy old slots
        foreach (Transform child in inventoryGRP.transform)
        {
            Destroy(child.gameObject);
        }

        // Recreate slots
        inventorySlots = new GameObject[capacity];
        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform);
            inventorySlots[i] = slot;
        }

        // Trim items if needed
        if (items.Count > capacity)
        {
            items.RemoveRange(capacity, items.Count - capacity);
        }
    }

    public void IncreaseCapacity(int addedSlots)
    {
        currentCapacity += addedSlots;
        SetInventorySpace(currentCapacity);
    }

    //  Link item data to UI
    void AddIntoInventoryUI(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount == 0)
            {
                GameObject placeholder = Instantiate(itemPlaceholder, inventorySlots[i].transform);
                placeholder.transform.localPosition = Vector3.zero;
                // Cast to CatRuntimeData
                CatRuntimeData catItem = item as CatRuntimeData;
                if (catItem != null)
                {
                    ItemUI itemUI = placeholder.GetComponent<ItemUI>();
                    if (itemUI != null)
                        itemUI.SetItem(catItem); // now icon is available
                }

                break;
            }
        }
    }
}
