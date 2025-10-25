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
        if (enteringCat == null)
        {
            Debug.LogError($"AddCatToContainer called with null CatUnit in container {containerIndex}");
            return;
        }

        occupyingCat = enteringCat;

        if (occupyingCat.catMovement == null)
            Debug.LogError($"Cat {occupyingCat.name} has no CatMovement component!");

        // Subscribe safely
        //occupyingCat.CatDeath += RemoveCatFromContainer;
        //occupyingCat.catMovement.onMove += RemoveCatFromContainer;

        OnCatEnter?.Invoke(occupyingCat);
        Debug.Log($"Cat {occupyingCat.runtimeData?.template?.name ?? "Unnamed"} entered container {containerIndex}");
    }


    public void RemoveCatFromContainer() //when catMovement.onMove, this function is called.
    {
        if (occupyingCat != null)
        {
            occupyingCat.catMovement.onMove -= RemoveCatFromContainer; //unsubscribe this first.
            occupyingCat = null; //make occupying null.

            OnCatExit?.Invoke(occupyingCat);
            Debug.Log($"Cat {occupyingCat.runtimeData.template.name} exited container {containerIndex}");
        }

    }

    public bool IsOccupied => occupyingCat != null;
}
