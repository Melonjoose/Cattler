using UnityEngine;

public class SummonManager : MonoBehaviour

{
    public static SummonManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonCat()
    {
        Debug.Log("Summoning Cat...");
        // Add summon logic here
    }
}
