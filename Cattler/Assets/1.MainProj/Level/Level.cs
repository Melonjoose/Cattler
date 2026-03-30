using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Level")]
public class Level : ScriptableObject
{
    public int level; //the game stage

    ///Normal Enemies///
    //Type of Enemies
    public List<EnemyData> spawnableList = new List<EnemyData>();

    //Frequency
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    //Quantity Per Spawn
    public int minEnemiesPerInterval = 0;
    public int maxEnemiesPerInterval = 1;

    //MaxEnemyThisLevel
    public int maxEnemies = 10;

    //Size
    public float minSize = 0.6f;
    public float maxSize = 1.4f;

    //Difficulty
    public int HPIncrease = 0; // + how much percentage in strength, if 10 = 10% increase of their base HP
    public int ATKIncrease = 0; // + how much percentage in strength, if 10 = 10% increase of their base HP
    public float ATKSPDIncrease = 0;
    public float MVSPDIncrease= 0;
    public float lootPercentage = 0;
}
