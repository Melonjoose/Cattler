using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public CatData debugCat; // assign in Inspector

    public List<Item> items = new List<Item>(); //this list stores references

    public int currentCapacity = 5;
    public int maxCapacity = 60;
    public GameObject inventoryGRP;
    public GameObject inventorySlotPrefab; // prefab for slots to instantiate into inventoryGRP
    public GameObject[] inventorySlots; //add from using a forloop detecting children of inventoryGRP. should be matching woth items list.

    public GameObject itemPlaceholder; // prefab for items to reference data and edited into items.

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        SetInventorySpace(currentCapacity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseCapacity(1);
        }
        
    }

    public bool Add(Item item)
    {
        if (items.Count >= currentCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        items.Add(item);
        Debug.Log($"Added {item.itemName} to inventory.");
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    void SetInventorySpace(int capacity)
    {
        if (capacity > maxCapacity)
        {
            currentCapacity = maxCapacity;
            return; // Do not exceed max capacity
        }

        // Destroy old slots
        foreach (Transform child in inventoryGRP.transform)
        {
            Destroy(child.gameObject);
        }

        inventorySlots = new GameObject[capacity]; // update array size to new capacity

        for (int i = 0; i < capacity; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGRP.transform);
            inventorySlots[i] = slot;
        }

        if (items.Count > capacity)
        {
            items.RemoveRange(capacity, items.Count - capacity);
        }
    }

    public void IncreaseCapacity(int addedslots)
    {
        currentCapacity += addedslots;
        SetInventorySpace(currentCapacity);
    }

    void AddIntoInventoryUI()
    {
        //instantiate itemPlaceholder as child of inventoryGRP
        //reference data from items list and reflect it to the prefab
    }
}
