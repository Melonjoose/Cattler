using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    private float timer = 0f;
    public List<EnemyData> availableEnemy;
    public GameObject enemyPrefab;
    public GameObject spawnLocation;

    private void Update()
    {
        timer += Time.deltaTime;

        Spawner();
    }

    public void SpawnEnemy(EnemyData data)
    {
        GameObject newEnemy = Instantiate(enemyPrefab,spawnLocation.transform.position, Quaternion.identity);

        // Optional: assign EnemyData to the instantiated object
        UnitController controller = newEnemy.GetComponent<UnitController>();
        if (controller != null)
        {
            controller.enemyData = data;
        }
    }

    void AddEnemyToSpawnList(EnemyData data)
    {
               availableEnemy.Add(data);
    }

    void CreateNewEnemy(EnemyData data)
    {

    }

    void Spawner()
    {
        if (timer >= spawnInterval)
        {
            if (availableEnemy.Count == 0) return;

            int randomIndex = Random.Range(0, availableEnemy.Count);
            SpawnEnemy(availableEnemy[randomIndex]);
            timer = 0f; // reset timer after spawning
        }
    }

    //Add any additional stats from higher difficulty


}