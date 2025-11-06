using UnityEngine;

public class Sword : Item
{
    public GameObject slash; //prefab

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseSkill()
    {
        Slash();
    }
    void Slash()
    {
        Vector3 spawnloc = catUnit.transform.position + Vector3.right; // Vector3(1,0,0) also works
        GameObject newSlash = Instantiate(slash, spawnloc, transform.rotation); // use rotation, not transform
        Slash SlashInfo = newSlash.GetComponent<Slash>();
        CatUnit catInfo = catUnit.GetComponent<CatUnit>();
        SlashInfo.catUnit = catInfo;
    }

    public void EquipSword(CatUnit user) 
    {
        catUnit = user;
    }
}
