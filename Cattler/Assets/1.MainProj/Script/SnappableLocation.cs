using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class SnappableLocation : MonoBehaviour
{

    //INCHARGE DATA TO BE READ..  HOLDS CONDTIONS TO BE ALLOWED TO PLACE IN HERE.
    public bool isOccupied;
    public InventoryIcon currentItem;
    public int SlotIndex = 0;

    [Header("Allowed Item Types")]
    public ItemType[] allowedTypes; // set in Inspector
    public enum ItemType
    {
        Cat,
        Weapon,
        Hat
    }

    public enum SlotType
    {
        InventoryList,
        TeamList,
        CharacterPreview,
        CatPreview,
        HatPreview,
        L_WeaponPreview,
        R_WeaponPreview
    }

    public SlotType slotType; // set in inspector per slot

}
