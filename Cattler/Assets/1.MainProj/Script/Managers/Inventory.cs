using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int capacity = 60;
    public List<Item> items = new List<Item>();


   
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public bool Add(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        items.Add(item);
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
