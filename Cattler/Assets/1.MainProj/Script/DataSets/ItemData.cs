using UnityEngine;

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public string description;
    public int value;

}

public enum ItemType { Cat, Weapon, Hat}

