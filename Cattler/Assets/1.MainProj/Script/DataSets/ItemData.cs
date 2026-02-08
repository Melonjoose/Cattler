using UnityEngine;

public class ItemData : ScriptableObject
{
    public GameObject prefab; // IconPrefab for inventory UI
    public GameObject prefab2; // The actual item prefab to spawn
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public string description;
    public int value;
    public float fvalue;

}

public enum ItemType { Cat, Weapon, Hat , Consumable}

