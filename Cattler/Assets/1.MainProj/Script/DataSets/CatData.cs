using UnityEngine;

public class CatData : ItemData
{

    //stats that should not change
    [Header("Unit Info")]
    public string skinName;
    public int catID;
    public Rarity Rarity;
    public string skillName;
    public string skillDesc;
    public int level;
    public int exp;

    [Header("Base Stats")]
    public int baseHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

}
    public enum Rarity { Common,Rare,Legendary}
