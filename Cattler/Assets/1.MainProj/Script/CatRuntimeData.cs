using UnityEngine;

[CreateAssetMenu(fileName = "RuntimeCat", menuName = "Items/RuntimeCat")]
public class CatRuntimeData : Item
{
    public string unitName;
    public Sprite unitIcon;
    public string unitType;
    [TextArea]
    public string unitDescription;
    public int level;

    public int EXP;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    // Copy stats from CatData template
    public void InitializeFrom(CatData template)
    {
        unitName = template.unitName;
        unitIcon = template.unitIcon;
        unitType = template.unitType;
        unitDescription = template.unitDescription;
        level = template.level;
        EXP = template.EXP;

        maxHealth = template.maxHealth;
        currentHealth = template.maxHealth; // start at full
        attackPower = template.attackPower;
        attackSpeed = template.attackSpeed;
        attackRange = template.attackRange;
        movementSpeed = template.movementSpeed;
    }
}