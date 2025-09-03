using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int capacity = 60;
    public List<Item> items = new List<Item>();

    public CatData debugCat; // assign in Inspector

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CatData catBase = Resources.Load<CatData>("Cats/BasicCat");
            CatRuntimeData runtimeCat = new CatRuntimeData(catBase);
            Add(catBase); // store reference to the base cat

            //CatRuntimeData runtimeCat = new CatRuntimeData(debugCat);
            //Add(debugCat);
        }
    }

    public bool Add(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        items.Add(item);
        Debug.Log($"Added {item.itemName} to inventory.");
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
