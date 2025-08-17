using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> availableEnemy;

    public void SpawnEnemy(EnemyData data)
    {
        //GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        //UnitController controller = newEnemy.GetComponent<UnitController>();
        //controller.catData = data;
        //controller.isCat = true;
    }

    



    //Add any additional stats from higher difficulty


}