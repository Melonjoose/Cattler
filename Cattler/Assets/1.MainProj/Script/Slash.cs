using UnityEngine;

public class Slash : ActiveAbility
{
    private void Start()
    {

    }
    void Update()
    {
        rb.MovePosition(rb.position + Vector2.right * movespeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyUnit enemy = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemy != null && catUnit != null )
        {
            int damage = Mathf.CeilToInt(catUnit.runtimeData.template.attackPower * 1.5f);
            enemy.TakeDamage(damage);
            DamageNumberManager.Instance.ShowDamage((int)damage, enemy.transform.localPosition);
            //Debug.Log("enemy hit by slash, taking" + damage);
        }
    }
}
