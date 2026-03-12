using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    public EnemySpawner enemySpawner; //reference in inspector
    public SpecialEnemySpawner specialEnemySpawner; //reference in inspector
    //handles all the enemy spawning logic //to reference enemy spawner & special enemy Spawner

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemySpawner.spawnerActive = false;
        specialEnemySpawner.spawnerActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        ReorderAllEnemies();
    }

    void ReorderAllEnemies()
    {
        enemySpawner.spawnedEnemies.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            enemySpawner.spawnedEnemies[i].transform.SetSiblingIndex(i);
        }

        specialEnemySpawner.spawnedEnemies.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < specialEnemySpawner.spawnedEnemies.Count; i++)
        {
            specialEnemySpawner.spawnedEnemies[i].transform.SetSiblingIndex(i);
        }
    }



    public void Level1()
    {
        
    }
    public void Level2()
    {

    }  

}
