
using UnityEngine;

public class MasterTester : MonoBehaviour
{
    public GameObject AdminButtons;
    public GameManager GameManager;
    public GameObject ES;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager = GetComponent<GameManager>();
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

            cat.GetComponent<CatUnit>().runtimeData.template.attackPower += 1;
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

        if (Input.GetKeyDown(KeyCode.S)) //summon a cat
        {
            SummonManager.instance.Summon();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Inventory.instance.IncreaseCapacity(+1);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            EnemySpawner.instance.SpawnEnemy(EnemySpawner.instance.spawnableList[0]);
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            //openInventory
            GameManager.OpenPage("Inventory");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //openInventory
            GameManager.CloseCurrentPage();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (ES.activeSelf)//if it is active,
            {
                ES.SetActive(false);
            }
            else
            {
                ES.SetActive(true);
            }
        }
    }

    void MakeMeRich()
    {
        Currency.instance.AddInk(999999999); // Add 1000 ink to the currency
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
