using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Units/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject prefab; // The actual enemy prefab to spawn

    public string enemyName;
    public Sprite icon;
    public EnemyType enemyType;

    public int health;
    
    public int attackPower;
    public float attackSpeed;
    public float attackRange;
    
    public float movementSpeed;


    /// Drop Loot Info ///
    public int minInkDrop;
    public int maxInkDrop;
    public int minCoreDrop;
    public int maxCoreDrop;
    public int expDrop;
   

    public enum EnemyType
    {
        Basic,
        Special,
        Boss,
    }
}