using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyUnit unit;
    public float moveSpeed = 5f; // Speed of movement

    public bool canWalk = true;

    [Header("Targeting")]
    public GameObject TargetCat; // Reference to current target

    public GameObject targetPoint; // Assign in inspector (child empty GameObject at attack point)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        unit = GetComponent<EnemyUnit>();
        moveSpeed = unit.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (unit.TargetCat != null  && canWalk) 
        {
            MovetowardsCat();
        }
    }

    private void MovetowardsCat()
    {
        if (TargetCat == null) return;

        Vector3 direction = (TargetCat.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

}
