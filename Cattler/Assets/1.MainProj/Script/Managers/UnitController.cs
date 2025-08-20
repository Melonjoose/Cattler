using UnityEngine;

public class UnitController : MonoBehaviour
{
    public bool isCat;
    public CatData catData;
    public EnemyData enemyData;

    private int currentHP;

    void Start()
    {
        if (isCat && catData != null)
        {
            InitCat(catData);
        }
        else if (!isCat && enemyData != null)
        {
            InitEnemy(enemyData);
        }
    }

    void InitCat(CatData data)
    {
        currentHP = data.health;
        // Set sprite, UI, skills etc
    }

    void InitEnemy(EnemyData data)
    {
        currentHP = data.health;
        // Similar logic
    }
}