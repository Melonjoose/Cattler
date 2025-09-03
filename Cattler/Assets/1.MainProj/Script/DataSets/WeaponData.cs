using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "items/Weapon")]
public class WeaponData : Item
{
    public string description;
    //Stats it gives to the cat
    public int health;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;

    public float movementSpeed;
    //public AbilityData[] abilities; // Optional
}