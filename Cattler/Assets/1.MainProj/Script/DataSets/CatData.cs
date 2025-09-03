using UnityEngine;

[CreateAssetMenu(fileName = "NewCat", menuName = "Items/Cat")]
public class CatData : Item
{
    public int level;
    public int EXP;
    public int health;
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    public float movementSpeed;
}
