using UnityEngine;

public class Sword : Item
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runtimeData = new ItemRuntimeData(runtimeData.template); 
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
        //add Slash Effect here.
    }
}
