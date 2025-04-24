using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items")]
public class ItemType : ScriptableObject
{
    public string Item;
    public Sprite icon;
    public int health;

    public int attackPower;
    public float attackSpeed;
    public float attackRange;

    public float movementSpeed;
    //public AbilityData[] abilities; // Optional
}