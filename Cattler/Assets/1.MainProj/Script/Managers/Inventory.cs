using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
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
    public GameObject[] teamSlots;  //manually added
    public GameObject[] previewSlots;  //manually added

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        InitializeInventorySpace(currentCapacity);
        SubscribeToSlots();
    }

    void SubscribeToSlots()
    {
        SubscribeToSlotList(inventorySlots);
        SubscribeToSlotList(teamSlots);
        SubscribeToSlotList(previewSlots);
    }

    void SubscribeToSlotList(IEnumerable<GameObject> slotObjects)
    {
        foreach (GameObject slotObj in slotObjects)
        {
            if (slotObj == null)
            {
                Debug.LogWarning($" A slot GameObject reference is missing in {name}.");
                continue;
            }

            SnappableLocation slot = slotObj.GetComponent<SnappableLocation>();
            if (slot == null)
            {
                Debug.LogWarning($" GameObject '{slotObj.name}' does not have a SnappableLocation component.");
                continue;
            }

        }
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

    public void InstantiateNewCat(CatData catData)
    {
        //Check for capacity
        if (inventoryList.Count >= currentCapacity)
        {
            Debug.LogWarning("Inventory full");
            return;
        }

        // Find first empty slot
        SnappableLocation emptySlot = GetFirstEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("No empty inventory slot found!");
            return;
        }

        //Instantiate the item prefab into the slot
        GameObject prefab = Instantiate(itemPlaceholder, emptySlot.transform);
        prefab.transform.SetParent(emptySlot.transform, false);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.localScale = Vector3.one;

        //Get its UI/Item script
        ItemUI newItem = prefab.GetComponent<ItemUI>();
        if (newItem == null)
        {
            Debug.LogError("ItemUI component missing on instantiated prefab!");
            return;
        }

        //Assign data
        newItem.itemData = catData;
        newItem.iconImage.sprite = catData.icon;
        InventoryIcon newItemIcon = newItem.GetComponent<InventoryIcon>();
        newItemIcon.originalSlot = emptySlot;
        newItemIcon.currentSlot = emptySlot;

        //Set as a Cat type item
        if (catData.type == ItemType.Cat)
        {
            prefab.name = catData.itemName;

            // Create and set up CatUnit
            CatUnit newCatUnit = prefab.AddComponent<CatUnit>();

            // Create runtime data based on template CatData
            newCatUnit.runtimeData = new CatRuntimeData(catData);

     
            // (Optional) link back to template if needed later
            // newCatUnit.runtimeData.template = catData;

            //Add to internal tracking
            inventoryList.Add(prefab);

            //Mark slot as occupied
            emptySlot.currentItem = prefab.GetComponent<InventoryIcon>();
            emptySlot.isOccupied = true;

            Debug.Log($" Spawned new Cat: {catData.itemName} into slot {emptySlot.name}");
        }
    }

    public void InstantiateNewWeapon(Item item)
    {
        if (inventoryList.Count >= currentCapacity) { Debug.LogWarning("Inventory full"); return; }
        SnappableLocation emptySlot = GetFirstEmptySlot();
        GameObject prefab = Instantiate(itemPlaceholder, emptySlot.transform);
        ItemUI newItem = prefab.GetComponent<ItemUI>();

        InventoryIcon newItemIcon = prefab.GetComponent<InventoryIcon>();
        newItemIcon.itemType = SnappableLocation.ItemType.Weapon;

        ItemType itemType = item.runtimeData.template.type;
        {
            prefab.name = item.runtimeData.template.itemName;
            newItem.itemData = item.runtimeData.template;
            newItem.iconImage.sprite = item.runtimeData.template.icon;
            prefab.AddComponent<Item>();
            Item newWeaponItem = prefab.GetComponent<Item>();
            newWeaponItem.runtimeData = new ItemRuntimeData(item.runtimeData.template); //create new runtimedata.

            newWeaponItem.runtimeData.health = item.runtimeData.health; //inherit item's randomized stats.
            newWeaponItem.runtimeData.attackPower = item.runtimeData.attackPower;
            newWeaponItem.runtimeData.attackSpeed = item.runtimeData.attackSpeed;
            newWeaponItem.runtimeData.attackRange = item.runtimeData.attackRange;
            newWeaponItem.runtimeData.movementSpeed = item.runtimeData.movementSpeed;
        }

        PlaceItem(newItemIcon, emptySlot);
    }
    public void Add(GameObject Item, SnappableLocation slot)  //when added new or when moving items around
    {
        if (slot == null)
        {
            Debug.LogWarning($"[Inventory] Tried to Add {Item.name} but slot was null.");
            return;
        }

        if (slot.slotType == SnappableLocation.SlotType.InventoryList)
        {
            inventoryList.Add(Item);
        }
        else if (slot.slotType == SnappableLocation.SlotType.TeamList)
        {
            teamList.Add(Item);
            if (Item.GetComponent<CatUnit>() != null)
            {
                CatUnit catUnit = Item.GetComponent<CatUnit>();
                TeamManager.instance.AddCatToWorld(catUnit, slot.SlotIndex);
            }
        }
        else if (slot.slotType == SnappableLocation.SlotType.CharacterPreview)
        {
            previewList.Add(Item);

            if (slot.gameObject.CompareTag("CatPreviewSlot"))
            {
                InventoryIcon itemIcon = slot.currentItem;
                SelectedItemDisplayUI.instance.ShowCatStats(itemIcon);
                Debug.Log(itemIcon);
            }
        }
    }
    public void Remove(GameObject item, SnappableLocation slot)
    {
        //Debug.Log($"[Inventory] Remove called for {item.name}");

        if (slot == null)
        {
            Debug.LogWarning($"[Inventory] Tried to remove {item.name} but slot was null.");
            return;
        }

        switch (slot.slotType)
        {
            case SnappableLocation.SlotType.InventoryList:
                inventoryList.Remove(item);
                break;

            case SnappableLocation.SlotType.TeamList:
                teamList.Remove(item);
                CatUnit cat = item.GetComponent<CatUnit>();
                TeamManager.instance.RemoveCatFromWorld(cat);
                break;


            case SnappableLocation.SlotType.CharacterPreview:
                previewList.Remove(item);
                SelectedItemDisplayUI.instance.RemoveCatStats();
                break;

            default:
                Debug.LogWarning($"[Inventory] Unknown slot type for {item.name}.");
                break;
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


    ///             MOVEMENT            ///

    public void PlaceItem(InventoryIcon item, SnappableLocation slot)
    {
        slot.isOccupied = true;
        slot.currentItem = item;

        Vector3 startWorldPos = item.transform.position; // Save current world position
        Vector3 endWorldPos = slot.transform.position;
        Vector3 targetScale = slot.transform.localScale;
        float tweenDuration = 0.2f;

        // Temporarily reparent to Canvas to stay on top
        Canvas canvas = item.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            item.transform.SetParent(canvas.transform, false);
            item.transform.position = startWorldPos; // Restore world position after reparenting
        }

        // Animate movement and scale
        LeanTween.move(item.gameObject, endWorldPos, tweenDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(item.gameObject, targetScale, tweenDuration).setEase(LeanTweenType.easeInOutQuad);

        // After animation, reparent to slot
        LeanTween.delayedCall(item.gameObject, tweenDuration, () =>
        {
            item.transform.SetParent(slot.transform, false);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = targetScale;

            item.currentSlot = slot;
            Add(item.gameObject, slot);
        });
    }


    public void RemoveItem(InventoryIcon item , SnappableLocation slot)
    {
        Remove(item.gameObject , slot); // pass real slot
    }

    public void SwapItem(InventoryIcon draggedItem , SnappableLocation draggedItemOriginalSlot , SnappableLocation newSlot)
    {
        InventoryIcon replacedItem = newSlot.currentItem;                // item currently in this slot
        SnappableLocation sourceSlot = draggedItem.originalSlot; // where dragged item came from

        // Step 1: remove both from their slots temporarily
        RemoveItem(draggedItem, draggedItemOriginalSlot);
        RemoveItem(replacedItem , newSlot);
    
        // Step 2: place dragged item into this slot
        PlaceItem(draggedItem , newSlot);

        // Step 3: place old item into dragged item's original slot (or fallback)
        PlaceItem(replacedItem, draggedItemOriginalSlot);
    }


    ///             MOVEMENT            ///
}
