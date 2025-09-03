using UnityEngine;

[System.Serializable]
public class CatRuntimeData
{
    public string catName;
    public Sprite icon;
    public int itemID;


    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    public CatRuntimeData(CatData baseData)
    {
        catName = baseData.itemName;
        icon = baseData.icon;
        itemID = baseData.itemID;

        maxHealth = baseData.health;
        currentHealth = baseData.health;
        attackPower = baseData.attackPower;
        attackSpeed = baseData.attackSpeed;
        attackRange = baseData.attackRange;
        movementSpeed = baseData.movementSpeed;
    }
}