using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthBar;
    public EnemyUnit enemyUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        enemyUnit = GetComponent<EnemyUnit>();
        healthBar.maxValue = enemyUnit.enemyData.health;
        healthBar.minValue = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = enemyUnit.currentHealth;
    }

}
