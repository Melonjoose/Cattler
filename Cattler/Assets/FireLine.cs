using UnityEngine;

public class FireLine : MonoBehaviour
{
    public float speed = 5f; // movement speed
    public Vector2 direction = new Vector2(1, -1); // 45� down-right

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // disable gravity
            rb.linearVelocity = direction.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit
        EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(null, 10, 5);
            Destroy(gameObject);
            return;
        }

        // Floor hit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
