using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Pages")]
    public GameObject inventory;
    public GameObject summonPage;

    [SerializeField]private Transform OpenedInventoryPOS;
    [SerializeField]private Transform ClosedInventoryPOS;
    [SerializeField]private Transform OpenedSummonPagePOS;
    [SerializeField]private Transform ClosedSummonPagePOS;

    public bool isInventoryOpen = false;
    public bool isSummonPageOpen = false;

    void Start()
    {
        CloseAllPages();
    }

    void Update()
    {
        
    }

    void CloseAllPages()
    {
        CloseSummonPage();
        CloseInventory();
    }

    public void OpenSummonPage()
    {
        isSummonPageOpen = true;
        summonPage.transform.position = OpenedSummonPagePOS.position;
        Debug.Log("Opening Summon Page");
    }

    public void CloseSummonPage()
    {
        isSummonPageOpen = false;
        summonPage.transform.position = ClosedSummonPagePOS.position;
        Debug.Log("Closing Summon Page");
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        inventory.transform.position = OpenedInventoryPOS.position;
        Debug.Log("Opening Inventory");
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventory.transform.position = ClosedInventoryPOS.position;
        Debug.Log("Closing Inventory");
    }
}
