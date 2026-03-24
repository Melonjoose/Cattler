using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static CatIconUI;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]private int currentLevelIndex = 0;
    [SerializeField]private float distanceTravelled;
    public Levels currentLevel;
    public float distanceToTriggerNextLevel;
    public Levels nextLevel; 
    
    [System.Serializable]
    public class Levels
    {
        public BasicSpawnerLevel basicAssignedLevel;
        public SpecialSpawnerLevel specialAssignedLevel;
        public float distanceToTrigger; //to trigger this level, if 1, == 1km distance travelled.
        public bool currentLevel; //is true when this level is triggered. all other level is turned off as false..
        public bool isAdded; //once added, don't add again 
    }

    [Header("Levels")]
    public Levels[] levels; // assign 5 in Inspector

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

        enemySpawner.spawnerActive = false; //start game at lobby hence false for now. when game starts, then set as true.
        specialEnemySpawner.spawnerActive = false;

        StartLevelOne();
    }

    public void StartLevelOne()
    {
        ResetAllLevel(); //set all level back to defaultstate.

        currentLevelIndex = 0;
        currentLevel = levels[currentLevelIndex];
        currentLevel.currentLevel = true;
        currentLevel.isAdded = true;

        enemySpawner.InitalizeLevelToSpawner(currentLevel.basicAssignedLevel);
        specialEnemySpawner.InitalizeLevelToSpawner(currentLevel.specialAssignedLevel);

        AssignNextLevel();
        AssignNextMileStone();
    }
    void Update()
    {
        distanceTravelled = TravelManager.instance.distanceTraveledUIvalue;

        UpdateNextLevelToCurrentLevel(); // check if we should advance
        ReorderAllEnemies();
    }


    void AssignNextMileStone()
    {
        if (nextLevel != null)
        {
            distanceToTriggerNextLevel = nextLevel.distanceToTrigger;
        }
    }

    void AssignNextLevel()
    {
        // Make sure we don’t go out of bounds
        if (currentLevelIndex + 1 < levels.Length)
        {
            nextLevel = levels[currentLevelIndex + 1];
            nextLevel.currentLevel = false;
        }
        else
        {
            nextLevel = null; // no more levels
        }
    }

    void UpdateNextLevelToCurrentLevel()
    {
        if (nextLevel != null && distanceTravelled >= distanceToTriggerNextLevel)
        {
            if (!nextLevel.isAdded)
            {
                UpdateLevelToCurrentLevel();
            }
        }
    }
    void UpdateLevelToCurrentLevel()
    {
        // Prevent going past the last level
        if (currentLevelIndex + 1 >= levels.Length)
        {
            Debug.Log("No more levels to advance to.");
            nextLevel = null;
            return;
        }

        currentLevelIndex += 1;
        currentLevel = levels[currentLevelIndex];
        currentLevel.currentLevel = true;
        currentLevel.isAdded = true;

        enemySpawner.InitalizeLevelToSpawner(currentLevel.basicAssignedLevel);
        specialEnemySpawner.InitalizeLevelToSpawner(currentLevel.specialAssignedLevel);

        AssignNextLevel();
        AssignNextMileStone();
    }


    public void ResetAllLevel()
    {
        foreach (var level in levels)
        {
            level.currentLevel = false;
            level.isAdded = false;
        }
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

}
