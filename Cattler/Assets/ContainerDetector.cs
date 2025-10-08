using UnityEngine;

public class ContainerDetector : MonoBehaviour
{
    public CatUnit occupyingCat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            occupyingCat = collision.GetComponent<CatUnit>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            if (occupyingCat == collision.GetComponent<CatUnit>())
                occupyingCat = null;
        }
    }

    public bool IsOccupied => occupyingCat != null;


}
