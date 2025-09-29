using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemySpawner : MonoBehaviour
{
    public static SpecialEnemySpawner instance;

    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;

    public float ChanceToSpawn = 5f;  //start at 1%

    public int minEnemiesPerInterval = 1;
    public int maxEnemiesPerInterval = 2;

    public int currentEnemyCount = 0;
    public int maxEnemies = 10;

    public float minSize = 0.6f;
    public float maxSize = 1.4f;

    private float timer = 0f;

    public List<GameObject> spawnableList = new List<GameObject>();

    public List<GameObject> spawnedEnemies = new List<GameObject>();

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
        if (TeamManager.instance.currentTeamSize == 0) return;

        timer += Time.deltaTime;


        if (currentEnemyCount <= maxEnemies) //if max enemies not reached
        {
            Spawner();
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Debug.Log("SpawnEnemy called");
        if (enemy == null)
        {
            Debug.LogError("Enemy prefab is null!");
            return;
        }
        // Pick a random offset within a circle
        Vector2 spawnOffset = Random.insideUnitCircle * radius;

        // Convert 2D offset to 3D position
        Vector3 spawnPos = spawnLocation.transform.position + new Vector3(spawnOffset.x, 0, 0f);

        float randomSize = Random.Range(minSize, maxSize);

        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        newEnemy.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        newEnemy.transform.SetParent(TravelManager.instance.floorGRP.transform, true);
        AddEnemyToSpawnedList(newEnemy);
        Debug.Log("enemyspawned");
    }

    void AddEnemyToSpawnList(GameObject enemy)
    {
        spawnableList.Add(enemy);
    }

    void Spawner()
    {
        if (timer >= Random.Range(minSpawnInterval, maxSpawnInterval))
        {
            if (spawnableList.Count == 0) return;

            int randomIndex = Random.Range(0, spawnableList.Count);

            int amountToSpawn = Random.Range(minEnemiesPerInterval, maxEnemiesPerInterval);

            if (amountToSpawn > maxEnemies - currentEnemyCount) //if amount to spawn exceeds max enemies, cap it
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
