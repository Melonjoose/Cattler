using Unity.VisualScripting;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public static PreviewManager instance;
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
        //if(catUnit) have weapon1 , weapon2, hat. AddItemToPreview(Item item)
    }

    public void AddItemToPreview(Item item , SnappableLocation slot)
    {
        Debug.Log("00");
        if (catUnit == null || item == null) return;
        Debug.Log("01");
        switch (item.runtimeData.template.itemType)
        {
            case ItemType.Weapon:
                if (slot.slotType == SnappableLocation.SlotType.CharacterPreview)
                {
                    Debug.Log("001");
                    if (slot.gameObject.CompareTag("WeaponPreviewSlot_R"))
                    {
                        Debug.Log("2");
                        weaponR = item;
                        SetWeaponPreview(item, "R_WeaponPlaceHolder");
                    }
                    else if (slot.gameObject.CompareTag("WeaponPreviewSlot_L"))
                    {
                        Debug.Log("3");
                        weaponL = item;
                        SetWeaponPreview(item, "L_WeaponPlaceHolder");
                    }
                }

                break;

            case ItemType.Hat:
                if (slot.slotType == SnappableLocation.SlotType.CharacterPreview)
                {
                    hat = item;
                }
                break;

            default:
                Debug.LogWarning("Unknown item type");
                break;
        }

    // Apply item to cat visually or functionally
    ItemShownWhenCatIsAdded();
    }

    private void SetWeaponPreview(Item item, string placeholderName)
    {
        Debug.Log("2");
        Transform placeholderTransform = catUnit.transform.Find("itemSprite/" + placeholderName);

        if (placeholderTransform != null)
        {
            GameObject weaponPlaceholderGO = placeholderTransform.gameObject;
            weaponPlaceholderGO.SetActive(true);

            UnityEngine.UI.Image imageComponent = weaponPlaceholderGO.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = item.runtimeData.template.icon;
            }
            else
            {
                Debug.LogWarning($"Image component not found on {placeholderName}.");
            }
        }
        else
        {
            Debug.LogWarning($"{placeholderName} not found under catUnit.");
        }
    }


    public void ItemFollowCat()
    {

    }

    public void ItemShownWhenCatIsAdded()
    {

    }
}
