using UnityEngine;
using System;
public class ContainerDetector : MonoBehaviour
{
    public CatUnit occupyingCat;
    public int containerIndex;
    public event Action<CatUnit> OnCatEnter;
    public event Action<CatUnit> OnCatExit;
    public event Action initializeContainer;

    private void OnEnable()
    {
        initializeContainer?.Invoke();
    }

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

    private void OnEnterContainer()
    {

    }


}
