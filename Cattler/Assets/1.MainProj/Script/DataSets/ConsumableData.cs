using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumable")]
public class ConsumableData : Item
{
    public string description;
    public int healAmount;
}