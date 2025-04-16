using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f; // Speed of movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {
        Vector2 direction = new Vector2(-1, 0); // Move left
        rb.linearVelocity = direction * moveSpeed;
    }
}
