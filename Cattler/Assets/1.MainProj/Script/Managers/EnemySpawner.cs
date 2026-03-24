using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EnemyData;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public bool spawnerActive = false;

    public Level currentLevel;

    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    public int minEnemiesPerInterval = 0;
    public int maxEnemiesPerInterval = 1;

    public int currentEnemyCount = 0;
    public int maxEnemies = 10;

    public float minSize = 0.6f;    
    public float maxSize = 1.4f;

    public float timer = 0f;

    public List<EnemyData> spawnableList = new List<EnemyData>();

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public GameObject spawnLocation;
    public int radius = 3;

    protected virtual void Awake() => instance = this;

    private void Update()
    {
        if(!spawnerActive) return;

        if (TeamManager.instance.currentTeamSize == 0) return;

        timer += Time.deltaTime;


        if(currentEnemyCount <= maxEnemies) //if max enemies not reached
        {
            Spawner();
        }
    }

    public void InitalizeLevelToSpawner(Level level)
    {
        currentLevel = level;
        minSpawnInterval = level.minSpawnInterval;
        maxSpawnInterval = level.maxSpawnInterval;
        minEnemiesPerInterval = level.minEnemiesPerInterval;
        maxEnemiesPerInterval = level.maxEnemiesPerInterval;
        maxEnemies = level.maxEnemies;
        minSize = level.minSize;
        maxSize = level.maxSize;
        AddEnemiesToSpawnableList();
    }

    void AddEnemiesToSpawnableList()
    {
        spawnableList.Clear(); // reset before adding

        foreach (EnemyData data in currentLevel.spawnableList)
        {
            if (data.enemyType == EnemyType.Basic && this is EnemySpawner)
            {
                spawnableList.Add(data);
            }
            else if (data.enemyType == EnemyType.Special && this is SpecialEnemySpawner)
            {
                spawnableList.Add(data);
            }
        }
    }


    void AdjustEnemyDifficulty(GameObject Enemy)
    {
        if(currentLevel == null)
        {
            return;
        }
        EnemyUnit enemyUnit = Enemy.GetComponent<EnemyUnit>();
        if(enemyUnit != null)
        {
            if(currentLevel.enemyAttackPercentage > 0)
            {
                enemyUnit.attackDamage = (int)(enemyUnit.enemyData.attackPower * (1f + currentLevel.enemyAttackPercentage / 100f));
            }
            if (currentLevel.enemyHealthPercentage > 0)
            {
                enemyUnit.maxHealth += (int)(enemyUnit.enemyData.health * (1f + currentLevel.enemyHealthPercentage / 100f)); //add enemy percentage of it's base attack
                enemyUnit.currentHealth = enemyUnit.maxHealth;
            }
            if (currentLevel.enemySpeedPercentage > 0)
            {
                enemyUnit.moveSpeed += (int)(enemyUnit.enemyData.movementSpeed * (1f + currentLevel.enemySpeedPercentage / 100f)); //add enemy percentage of it's base attack
            }
        }
    }

    public void SpawnEnemy(EnemyData data)
    {
        // Pick a random offset within a circle
        Vector2 spawnOffset = Random.insideUnitCircle * radius;

        // Convert 2D offset to 3D position
        Vector3 spawnPos = spawnLocation.transform.position + new Vector3(spawnOffset.x, 0, 0f);

        //enemy can spawn at a random size
        float randomSize = Random.Range(minSize, maxSize);

        GameObject newEnemy = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        AdjustEnemyDifficulty(newEnemy);
        newEnemy.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        newEnemy.transform.SetParent(TravelManager.instance.floorGRP.transform, true);
        AddEnemyToSpawnedList(newEnemy);
    }

    void Spawner()
    {
        if (timer >= Random.Range(minSpawnInterval,maxSpawnInterval))
        {
            if (spawnableList.Count == 0) return;

            int randomIndex = Random.Range(0, spawnableList.Count);

            int amountToSpawn = Random.Range(minEnemiesPerInterval, maxEnemiesPerInterval);

            if(amountToSpawn > maxEnemies - currentEnemyCount) //if amount to spawn exceeds max enemies, cap it
            {
                amountToSpawn = maxEnemies - currentEnemyCount;
            }

            for (int i = 0; i < amountToSpawn; i++)
            {
                SpawnEnemy(spawnableList[randomIndex]);
            }

            //add spawned enemies to list to spawnedenemies List

            timer = 0f; // reset timer after spawning
        }
    }

    public void AddEnemyToSpawnedList(GameObject spawnedEnemy)
    {
        spawnedEnemies.Add(spawnedEnemy);
        UpdateSpawnedEnemy();
    }

    public void RemoveSpawnedEnemies(GameObject spawnedEnemy)
    {
        currentEnemyCount -= 1;
        spawnedEnemies.Remove(spawnedEnemy);
        UpdateSpawnedEnemy();
    }

    public virtual void UpdateSpawnedEnemy()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            currentEnemyCount = spawnedEnemies.Count;
        }
    }
    //Add any additional stats from higher difficulty

    public void ClearAllSpawnedEnemies()
    {
        if(spawnedEnemies.Count == 0) return;

        //destory all spawned enemies in spawnedenemies List
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        currentEnemyCount = 0;
        spawnedEnemies.Clear();
        UpdateSpawnedEnemy();
    }
}