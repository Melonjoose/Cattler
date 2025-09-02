using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MasterTester : MonoBehaviour
{
    public GameObject AdminButtons;

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

        if(Input.GetKeyDown(KeyCode.M)) //give Currency to the player
        {
            MakeMeRich();
        }

        if (Input.GetKeyDown(KeyCode.L)) //give Currency to the player
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.P)) //give Currency to the player
        {
            OpenDebugList();
        }
    }

    void MakeMeRich()
    {
        Currency.instance.AddInk(1000); // Add 1000 ink to the currency
        Currency.instance.AddEXP(500); // Add 500 EXP to the currency
        Currency.instance.AddCore(10); // Add 10 core to the currency
    }

    void OpenDebugList()
    {
        if (!AdminButtons.activeSelf) { AdminButtons.SetActive(true); return; }
        else if (AdminButtons.activeSelf) { AdminButtons.SetActive(false); return; }
    }

    void StartGame()
    {

    }
}
