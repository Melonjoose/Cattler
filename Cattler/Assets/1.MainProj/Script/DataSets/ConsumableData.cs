using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumable")]
public class ConsumableData : ItemData
{
    public string itemNameShort;
    public GameObject prefab;
}