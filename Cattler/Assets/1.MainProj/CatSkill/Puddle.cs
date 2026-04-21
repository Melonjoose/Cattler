using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public Collider2D puddleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.SetActive(true);
        puddleCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private HashSet<EnemyUnit> debuffedEnemies = new HashSet<EnemyUnit>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && puddleCollider.gameObject.activeSelf)
        {
            EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
            if (enemyUnit != null && !debuffedEnemies.Contains(enemyUnit) && enemyUnit.isSlowed == false)
            {
                debuffedEnemies.Add(enemyUnit);
                DebuffManager.instance.ApplyDebuff(enemyUnit.gameObject, DebuffManager.instance.slow, 7f);
            }
        }
    }
}
