using UnityEngine;

[System.Serializable]
public class ItemRuntimeData
{
    //changable stats during gameplay. to be saved and stored into new datasets

    public WeaponData template;
    [Header("Base Stats")]
    public int health;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;

    public ItemRuntimeData(WeaponData template)
    {
        this.template = template;

        this.health = template.health;
        this.attackPower = template.attackPower;
        this.attackSpeed = template.attackSpeed;
        this.attackRange = template.attackRange;
        this.movementSpeed = template.movementSpeed;
    }

}
