using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PreviewManager : MonoBehaviour
{
    public static PreviewManager instance;

    public SnappableLocation hatSlot , weaponLSlot , weaponRSlot ;
    public CatUnit catUnit;
    public Item hat;
    public Item weaponL;
    public Item weaponR;
    // this is to control items to be added onto the catUnit.
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCatToPreview(CatUnit cat)
    {
        catUnit = cat;

        if (catUnit.hat != null)
        {
            AddItemToPreview(catUnit.hat, hatSlot);
        }

        if (catUnit.weaponL != null)
        {
            AddItemToPreview(catUnit.weaponL, weaponLSlot);
        }

        if (catUnit.weaponR != null)
        {
            AddItemToPreview(catUnit.weaponR, weaponRSlot);
        }

        //check catUnit's Hat,Wep1,Wep2 for data. if have add them to respective slots.
    }

    public void RemoveCatFromPreview(CatUnit cat , SnappableLocation slot)
    {
        catUnit = null;

        if (hat != null)
        {
            Inventory.instance.RemoveItemFromPreviewList(hat.gameObject);
            ItemFollowCat(hat);
        }
        if (weaponL != null)
        {
            Inventory.instance.RemoveItemFromPreviewList(weaponL.gameObject);
            ItemFollowCat(weaponL);
        }
        if (weaponR != null)
        {
            Inventory.instance.RemoveItemFromPreviewList(weaponR.gameObject);
            ItemFollowCat(weaponR);
        }
    }

    public Transform itemEquippedGroup; // Assign this in the Inspector

    public void ItemFollowCat(Item item)
    {
        if (item == null) return;

        // Move item under itemEquippedGroup for hierarchy organization
        if (itemEquippedGroup != null)
        {
            item.transform.SetParent(itemEquippedGroup);
            item.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("itemEquippedGroup is not assigned.");
            item.transform.position = Vector3.zero;
        }

        item.gameObject.SetActive(false); // Hide item temporarily

        // Clear references
        if (item == hat)
        {
            hat = null;
        }
        else if (item == weaponL)
        {
            weaponL = null;
        }
        else if (item == weaponR)
        {
            weaponR = null;
        }
    }

    public void AddItemToPreview(Item item, SnappableLocation slot)
    {
        if (catUnit == null || item == null || slot == null) return;

        switch (item.runtimeData.template.itemType)
        {
            case ItemType.Weapon:
                if (slot.slotType == SnappableLocation.SlotType.CharacterPreview)
                {
                    if (slot.gameObject.CompareTag("WeaponPreviewSlot_R"))
                    {
                        weaponR = item;
                        ReparentItemToSlot(item, slot);
                        SetItemToCat(item, "R_WeaponPlaceHolder");
                    }
                    else if (slot.gameObject.CompareTag("WeaponPreviewSlot_L"))
                    {
                        weaponL = item;
                        ReparentItemToSlot(item, slot);
                        SetItemToCat(item, "L_WeaponPlaceHolder");
                    }
                }
                break;

            case ItemType.Hat:
                if (slot.slotType == SnappableLocation.SlotType.CharacterPreview)
                {
                    if (slot.gameObject.CompareTag("HatPreviewSlot"))
                    {
                        hat = item;
                        ReparentItemToSlot(item, slot);
                        SetItemToCat(item, "T_HatPlaceHolder");
                    }
                }
                break;

            default:
                Debug.LogWarning("Unknown item type");
                break;
        }
    }

    private void ReparentItemToSlot(Item item, SnappableLocation slot)
    {
        item.transform.SetParent(slot.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.gameObject.SetActive(true); // Ensure it's visible
    }

    private void SetItemToCat(Item item, string placeholderName)
    {
        Transform placeholderTransform = catUnit.transform.Find("itemSprite/" + placeholderName);

        if (placeholderTransform != null)
        {
            GameObject itemPlaceholderGO = placeholderTransform.gameObject;
            itemPlaceholderGO.SetActive(true);

            UnityEngine.UI.Image imageComponent = itemPlaceholderGO.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = item.runtimeData.template.icon;
            }
            else
            {
                Debug.LogWarning($"Image component not found on {placeholderName}.");
            }

            item.gameObject.SetActive(true); // Ensure item is visible

            if (item == hat) catUnit.hat = item; 
            if (item == weaponL) catUnit.weaponL = item; 
            if (item == weaponR) catUnit.weaponR = item;

        }
        else
        {
            Debug.LogWarning($"{placeholderName} not found under catUnit.");
        }
    }

    public void RemoveItem(SnappableLocation slot)
    {
        Debug.Log("Removing Item");
        if (slot.currentItem == null || catUnit == null) return;

        InventoryIcon iconItemToRemove = slot.currentItem;
        Item itemToRemove = iconItemToRemove.GetComponent<Item>();
        if (itemToRemove == hat)
        {
            hat = null;
            catUnit.hat = null;
            ClearItemFromCat("T_HatPlaceHolder");
        }
        else if (itemToRemove == weaponL)
        {
            Debug.Log("Removing Left Weapon");
            weaponL = null;
            catUnit.weaponL = null;
            ClearItemFromCat("L_WeaponPlaceHolder");
        }
        else if (itemToRemove == weaponR)
        {
            weaponR = null;
            catUnit.weaponR = null;
            ClearItemFromCat("R_WeaponPlaceHolder");
        }
    }

    private void ClearItemFromCat(string placeholderName)   //Visuals for now
    {
        Transform placeholderTransform = catUnit.transform.Find("itemSprite/" + placeholderName);
        if (placeholderTransform != null)
        {
            GameObject itemPlaceholderGO = placeholderTransform.gameObject;
            itemPlaceholderGO.SetActive(false);
        }
    }


public void ItemFollowCatWhenRemoved()
{
    if (catUnit == null) return;

    if (hat != null) ClearItemFromCat("T_HatPlaceHolder");
    if (weaponL != null) ClearItemFromCat("L_WeaponPlaceHolder");
    if (weaponR != null) ClearItemFromCat("R_WeaponPlaceHolder");

    hat = null;
    weaponL = null;
    weaponR = null;
}


}
