using UnityEngine;

public class Deleter : MonoBehaviour
{
    private Collider2D collider2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Destroy(other.gameObject);
        }
    }
}
