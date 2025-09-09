using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    public int minEnemiesPerInterval = 1;
    public int maxEnemiesPerInterval = 2;

    public int currentEnemyCount = 0;
    public int maxEnemies = 10;

    private float timer = 0f;
    
    public List<EnemyData> enemyList;

    public List<EnemyData> spawnableList = new List<EnemyData>();

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public GameObject enemyPrefab;
    public GameObject spawnLocation;
    public int radius = 3;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;


        if(currentEnemyCount <= maxEnemies) //if max enemies not reached
        {
            Spawner();
        }
    }

    public void SpawnEnemy(EnemyData data)
    {
        // Pick a random offset within a circle
        Vector2 spawnOffset = Random.insideUnitCircle * radius;

        // Convert 2D offset to 3D position
        Vector3 spawnPos = spawnLocation.transform.position + new Vector3(spawnOffset.x, 0, 0f);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        AddEnemyToSpawnedList(newEnemy);
    }

    void AddEnemyToSpawnList(EnemyData data)
    {
            spawnableList.Add(data);
    }

    void CreateNewEnemy(EnemyData data)
    {

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

    void AddEnemyToSpawnedList(GameObject spawnedEnemy)
    {
        spawnedEnemies.Add(spawnedEnemy);
        UpdateSpawnedEnemy();
    }

    public void RemoveSpawnedEnemies(GameObject spawnedEnemy)
    {
        spawnedEnemies.Remove(spawnedEnemy);
        UpdateSpawnedEnemy();
    }

    void UpdateSpawnedEnemy()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            currentEnemyCount = spawnedEnemies.Count;
        }
    }
    //Add any additional stats from higher difficulty


}