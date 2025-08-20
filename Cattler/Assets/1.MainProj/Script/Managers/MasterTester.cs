using UnityEngine;

public class MasterTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) //give extra stats to individual cat called Cat1.
            //since it's running on a runtimeData, it will not affect any other cats since I am not changing the baseData.
        {
            Debug.Log("Space key was pressed!");
            //give +1 attackPower to Cat
            GameObject cat = GameObject.Find("Cat1");

            cat.GetComponent<CatUnit>().runtimeData.attackPower += 1;
        }
    }
}
