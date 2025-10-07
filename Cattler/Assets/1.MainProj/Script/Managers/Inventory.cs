using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private bool isInitialized = false;
    public List<CatRuntimeData> inventoryList = new List<CatRuntimeData>();
    public List<CatRuntimeData> teamList = new List<CatRuntimeData>();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
 
        }

    }

    public void Add(CatRuntimeData newCat)
    {
        if (inventoryList.Count >= currentCapacity) { Debug.LogWarning("Inventory full"); return; }
        inventoryList.Add(newCat);
        Debug.Log($"Added {newCat.template.itemName} to inventory.");
    }

    public void Remove(CatRuntimeData newCat)
    {
        if (inventoryList.Contains(newCat))
        {
            int index = inventoryList.IndexOf(newCat);
            if (index >= 0)
            {
                inventoryList[index] = null; // mark the slot empty
            }
        }
        else if (teamList.Contains(newCat))
        {
            teamList.Remove(newCat);
        }
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

        // Make sure inventoryList always matches capacity in size
        while (inventoryList.Count < capacity)
            inventoryList.Add(null);

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

}
