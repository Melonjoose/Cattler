using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionManagerV1 : MonoBehaviour
{
    public List<ContainerDetector> containers = new List<ContainerDetector>();
    private List<CatUnit> registeredCats = new List<CatUnit>();

    void Awake()
    {
        // Find all containers automatically
        containers.AddRange(GetComponentsInChildren<ContainerDetector>());
    }

    /// <summary>
    /// Called when a new cat spawns or joins the scene.
    /// </summary>
    public void RegisterCat(CatUnit newCat)
    {
        if (newCat == null || registeredCats.Contains(newCat))
            return;

        registeredCats.Add(newCat);
        // Subscribe to death event
        newCat.CatDeath += () => RemoveFromContainer(newCat);
    }

    /// <summary>
    /// Optionally unregister a cat manually (e.g. on scene unload or despawn)
    /// </summary>
    public void UnregisterCat(CatUnit cat)
    {
        if (cat == null) return;

        if (registeredCats.Contains(cat))
        {
            registeredCats.Remove(cat);
            cat.CatDeath -= () => RemoveFromContainer(cat); // optional
        }
    }

    public ContainerDetector GetContainerByIndex(int index)
    {
        if (index < 0 || index >= containers.Count)
        {
            Debug.LogWarning("Invalid container index requested.");
            return null;
        }
        return containers[index];
    }

    public ContainerDetector FindContainerWithCat(CatUnit targetCat)
    {
        foreach (var container in containers)
        {
            if (container.occupyingCat == targetCat)
                return container;
        }
        return null;
    }

    public ContainerDetector FindContainerNear(Vector3 position)
    {
        foreach (var container in containers)
        {
            if (Vector3.Distance(container.transform.position, position) < 1f)
                return container;
        }
        return null;
    }

    private void RemoveFromContainer(CatUnit deadCat)
    {
        var container = FindContainerWithCat(deadCat);
        if (container != null)
        {
            Debug.Log($"Removing {deadCat.name} from {container.name}");
            container.occupyingCat = null;
        }

        // Optionally cleanup the cat reference
        UnregisterCat(deadCat);
    }
}
