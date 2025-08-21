using UnityEngine;

[System.Serializable]
public class CatRuntimeData
{
    public string catName;
    public Sprite icon;
    public int currentHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    public CatRuntimeData(CatData baseData)
    {
        catName = baseData.catName;
        icon = baseData.icon;
        currentHealth = baseData.health;
        attackPower = baseData.attackPower;
        attackSpeed = baseData.attackSpeed;
        attackRange = baseData.attackRange;
        movementSpeed = baseData.movementSpeed;
    }
}