using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    //Stats it gives to the cat
    [Header("Ability")]
    public string AbilityName;
    public string AbilityDesc;

    [Header("Stats to give to cat")]
    public int health;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;

    public float movementSpeed;
    //public AbilityData[] abilities; // Optional
}