using UnityEngine;

public class Slash : MonoBehaviour
{
    public float movespeed = 3f;
    public float lifetime = 0.7f;

    private float timer = 0f;

    public CapsuleCollider2D col;
    public Rigidbody2D rb;

    public CatUnit CatUnit;

    private void Start()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent <Rigidbody2D>();
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
        if (enemy != null && CatUnit != null )
        {
            int damage = Mathf.CeilToInt(CatUnit.runtimeData.attackPower * 1.5f);
            enemy.TakeDamage(damage);
            DamageNumberManager.Instance.ShowDamage((int)damage, enemy.transform.localPosition);
            //Debug.Log("enemy hit by slash, taking" + damage);
        }
    }
}
