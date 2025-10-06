using UnityEngine;

public class CatData : ItemData
{
    [Header("Unit Info")]
    public string unitType;
    public int level;
    public int exp;

    [Header("Base Stats")]
    public int baseHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;
}
