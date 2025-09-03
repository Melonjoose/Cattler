using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]private bool isEnemy;
    [SerializeField]private bool isCat;

    public Slider healthBar;
    public EnemyUnit enemyUnit;
    public CatUnit catUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitChecker();
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnemy && enemyUnit != null)
        {
            healthBar.value = enemyUnit.currentHealth;
        }
        else if(isCat && catUnit != null)
        {
            healthBar.value = catUnit.runtimeData.currentHealth;
            healthBar.maxValue = catUnit.runtimeData.maxHealth;
        }
    }

    void SetCatHealthBar()
    {
        CatUnit Cat = GetComponent<CatUnit>();
        if (Cat == null) { return; }
        healthBar = GetComponentInChildren<Slider>();
        catUnit = GetComponent<CatUnit>();
        healthBar.maxValue = catUnit.runtimeData.maxHealth;
        healthBar.minValue = 0f;
        isCat = true;
        Debug.Log("Cat Healthbar Set");
    }

    void SetEnemyHealthBar()
    {
        EnemyUnit Enemy = GetComponent<EnemyUnit>();
        if (Enemy == null) { return; }
        healthBar = GetComponentInChildren<Slider>();
        enemyUnit = GetComponent<EnemyUnit>();
        healthBar.maxValue = enemyUnit.enemyData.health;
        healthBar.minValue = 0f;
        isEnemy = true;
        Debug.Log("Enemy Healthbar Set");
    } 

    void UnitChecker()
    {
        CatUnit Cat = GetComponent<CatUnit>();
        EnemyUnit Enemy = GetComponent<EnemyUnit>();
        if (Cat != null) { SetCatHealthBar(); return; }
        if (Enemy != null) { SetEnemyHealthBar(); return; }

    }
}
