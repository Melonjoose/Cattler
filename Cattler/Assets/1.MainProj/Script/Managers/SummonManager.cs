using System;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance;

    public Action OnSummon;
    [System.Serializable]
    public class GachaPoolEntry
    {
        public CatData catData;
        public float weight; // probability weight
    }

    public GachaPoolEntry[] gachaPool; // assign in inspector

    private void Awake()
    {
        instance = this;
    }

    public CatData Roll()
    {
        float totalWeight = 0f;
        foreach (var entry in gachaPool)
            totalWeight += entry.weight;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in gachaPool)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.catData;
        }

        return null; // should never happen
    }

    public void Summon()
    {
        if (TeamManager.instance.currentTeamSize >= TeamManager.instance.availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return;
        }

        CatData rolledCat = Roll();
        if (rolledCat == null)
        {
            Debug.LogError("No cat was rolled!");
            return;
        }

        CatRuntimeData runtimeCat = new CatRuntimeData(rolledCat);

        //  2. Add runtime cat to inventory
        //Inventory.instance.Add(runtimeCat);  //might change to add cats

        //TeamManager.instance.AddCatToWorld(runtimeCat); // belongs to TeamManager or Inventory? whenever cat is added to Inventory.TeamList.
        
        Debug.Log($"Summoned {rolledCat.itemName}!");
        OnSummon?.Invoke(); // Notify listeners
        //send to Inventory to add into inventory
    }

    public void TestSummon()
    {
        if (TeamManager.instance.currentTeamSize >= TeamManager.instance.availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return;
        }

        CatData rolledCat = Roll();
        if (rolledCat == null)
        {
            Debug.LogError("No cat was rolled!");
            return;
        }

        CatRuntimeData runtimeCat = new CatRuntimeData(rolledCat);

        //  2. Add runtime cat to inventory
        //Inventory.instance.Add(runtimeCat);  //might change to add cats

        TeamManager.instance.AddCatToWorld(runtimeCat);

        Debug.Log($"Summoned {rolledCat.itemName}!");
        OnSummon?.Invoke(); // Notify listeners
        //send to Inventory to add into inventory
    }
}
