using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Pages")]
    public GameObject lobbyPage;
    public GameObject inventoryPage;
    public GameObject summonPage;
    public GameObject upgradePage;

    [SerializeField] private Transform OpenedLobbyPOS;
    [SerializeField] private Transform ClosedLobbyPOS;

    [SerializeField]private Transform OpenedInventoryPOS;
    [SerializeField]private Transform ClosedInventoryPOS;

    [SerializeField]private Transform OpenedSummonPagePOS;
    [SerializeField]private Transform ClosedSummonPagePOS;

    [SerializeField] private Transform OpenedUpgradePagePOS;
    [SerializeField] private Transform ClosedUpgradePagePOS;

    public bool isLobbyOpen = false;
    public bool isInventoryOpen = false;
    public bool isSummonPageOpen = false;
    public bool isUpgradePageOpen = false;

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
        CloseLobby();
        CloseUpgradePage();
    }

    public void OpenSummonPage()
    {
        isSummonPageOpen = true;
        summonPage.transform.position = OpenedSummonPagePOS.position;
        //Debug.Log("Opening Summon Page");
    }

    public void CloseSummonPage()
    {
        isSummonPageOpen = false;
        summonPage.transform.position = ClosedSummonPagePOS.position;
        //Debug.Log("Closing Summon Page");
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        inventoryPage.transform.position = OpenedInventoryPOS.position;
        //Debug.Log("Opening Inventory");
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventoryPage.transform.position = ClosedInventoryPOS.position;
        //Debug.Log("Closing Inventory");
    }

    public void OpenLobby()
    {
        isLobbyOpen = true;
        lobbyPage.transform.position = OpenedLobbyPOS.position;
        //Debug.Log("Opening Lobby");
    }
    public void CloseLobby()
    {
        isLobbyOpen = false;
        lobbyPage.transform.position = ClosedLobbyPOS.position;
        //Debug.Log("Closing Lobby");
    }

    public void OpenUpgradePage()
    {
        isUpgradePageOpen = true;
        upgradePage.transform.position = OpenedUpgradePagePOS.position;
        //Debug.Log("Opening Upgrade Page");
    }

    public void CloseUpgradePage()
    {
        isUpgradePageOpen = false;
        upgradePage.transform.position = ClosedUpgradePagePOS.position;
        //Debug.Log("Closing Upgrade Page");
    }
}