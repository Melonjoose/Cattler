using UnityEngine;

[CreateAssetMenu(fileName = "NewCat", menuName = "Items/Cat")]
public class CatData : Item

{
    //add a collapsible header for thid 3 fields called "Unit Info"
    [Header("Unit Info")]
    public string unitName;
    public Sprite unitIcon;
    public string unitType;
    [TextArea]
    public string unitDescription;
    public int level;

    public int EXP;

    //stats
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    //  Add this method to make a copy
    public CatData Clone()
    {
        CatData clone = ScriptableObject.CreateInstance<CatData>();
        clone.unitName = this.unitName;
        clone.unitIcon = this.unitIcon;
        clone.unitType = this.unitType;
        clone.unitDescription = this.unitDescription;
        clone.level = this.level;
        
        clone.EXP = this.EXP;

        clone.maxHealth = this.maxHealth;
        clone.currentHealth = this.currentHealth;
        clone.attackPower = this.attackPower;
        clone.attackSpeed = this.attackSpeed;
        clone.attackRange = this.attackRange;
        clone.movementSpeed = this.movementSpeed;

        return clone;
    }

}
