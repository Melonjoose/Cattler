using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Units/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite icon;
    public int health;
    public int attackPower;
    public float moveSpeed;
    //public EnemyType type;
}