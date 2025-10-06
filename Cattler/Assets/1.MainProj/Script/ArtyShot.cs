using UnityEngine;

public class ArtyShot : MonoBehaviour
{
    public int damage;
    public float movespeed = 3f;
    public float lifetime = 0.7f;

    private float timer = 0f;

    public CapsuleCollider2D col;
    public Rigidbody2D rb;

    public EnemyUnit unit;

    public void Start()
    {
        damage = unit.attackDamage;
    }
    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime) Destroy(gameObject);
    }

    public void Launch(Vector2 target, EnemyUnit shooter)
    {
        unit = shooter;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 start = transform.position;
        Vector2 toTarget = target - start;

        float flightTime = 4f; // how long the projectile should fly
        float gravity = Physics2D.gravity.y * rb.gravityScale; // negative value

        // Calculate velocity needed to arrive at target in given time
        float vx = toTarget.x / flightTime;
        float vy = (toTarget.y - 0.5f * gravity * flightTime * flightTime) / flightTime;

        rb.linearVelocity = new Vector2(vx, vy);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            CatUnit cat = other.GetComponent<CatUnit>();
            if (cat != null)
            {
                cat.TakeDamage(damage);
                DamageNumberManager.Instance.ShowDamage(damage, cat.transform.position);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Floor"))
        {
            Debug.Log("Hit floor!");
            Destroy(gameObject);
        }
    }
}
