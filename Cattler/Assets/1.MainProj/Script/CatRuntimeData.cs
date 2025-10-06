using UnityEngine;

[System.Serializable]
public class CatRuntimeData
{
    public CatData template;
    [Header("Base Stats")]
    public int currentHealth;
    public int maxHealth;
    public int level;
    public int exp;

    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    public CatRuntimeData(CatData template)
    {
        this.template = template;
        
        this.currentHealth = template.baseHealth;
        this.maxHealth = template.baseHealth;
        this.level = template.level;
        this.exp = 0;

        this.attackPower = template.attackPower;
        this.attackSpeed = template.attackSpeed;
        this.attackRange = template.attackRange;
        this.movementSpeed = template.movementSpeed;
    }

    public void GainExp(int amount)
    {
        exp += amount;
        // Example level-up logic
        if (exp >= 100)
        {
            exp -= 100;
            level++;
            currentHealth = maxHealth; // restore health on level up
        }
    }
}
