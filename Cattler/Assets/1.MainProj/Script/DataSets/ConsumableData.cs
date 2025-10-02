using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumable")]
public class ConsumableData : Item
{
    public string itemName;
    public string itemNameShort;
    public Sprite icon;
    public string description;
    public GameObject prefab;
    public int value;
}