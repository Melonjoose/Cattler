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
         //but doesnt work
    }

    //using Adding/Removing function to detect cats entering/exiting container
    public void AddCatToContainer(CatUnit enteringCat)
    {
        occupyingCat = enteringCat;
        occupyingCat.CatDeath += RemoveCatFromContainer; // Subscribe to CatDeath event // so when cat dies. run RemoveCatFromContainer
        occupyingCat.catMovement.onMove += RemoveCatFromContainer;
        OnCatEnter?.Invoke(occupyingCat);
        Debug.Log($"Cat {occupyingCat.runtimeData.template.name} entered container {containerIndex}");
    }

    public void RemoveCatFromContainer()
    {
        if (occupyingCat != null)
        {
            occupyingCat = null;

            occupyingCat.catMovement.onMove -= RemoveCatFromContainer;

            OnCatExit?.Invoke(occupyingCat);
            Debug.Log($"Cat {occupyingCat.runtimeData.template.name} exited container {containerIndex}");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision) // not really working. It's becuz
    {
        if (occupyingCat == null)
            return; // No cat assigned yet, skip

        // Check if the collider belongs to the occupyingCat
        if (collision.gameObject == occupyingCat.gameObject)
        {
            CatMovement catMovement = occupyingCat.GetComponent<CatMovement>();
            if (catMovement != null)
            {
                catMovement.inPosition = true;
                Debug.Log($"{occupyingCat.name} reached its container position!");
            }
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            if (occupyingCat == collision.GetComponent<CatUnit>())
                occupyingCat = null;
        }
    }
    */
    public bool IsOccupied => occupyingCat != null;
}
