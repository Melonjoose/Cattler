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
        // Sort normal enemies by X
        enemySpawner.spawnedEnemies.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < enemySpawner.spawnedEnemies.Count; i++)
        {
            var enemy = enemySpawner.spawnedEnemies[i];
            enemy.transform.SetSiblingIndex(i);

            // Find MeshRenderer on Spine child
            MeshRenderer mr = enemy.transform.Find("Spine GameObject")?.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                // leftmost (lowest x) gets highest order
                mr.sortingOrder = enemySpawner.spawnedEnemies.Count - i;
            }
        }

        // Sort special enemies by X
        specialEnemySpawner.spawnedEnemies.Sort((a, b) =>
            a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < specialEnemySpawner.spawnedEnemies.Count; i++)
        {
            var enemy = specialEnemySpawner.spawnedEnemies[i];
            enemy.transform.SetSiblingIndex(i);

            MeshRenderer mr = enemy.transform.Find("Spine GameObject")?.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.sortingOrder = specialEnemySpawner.spawnedEnemies.Count - i;
            }
        }
    }



    public void Level1()
    {
        
    }
    public void Level2()
    {

    }  

}
