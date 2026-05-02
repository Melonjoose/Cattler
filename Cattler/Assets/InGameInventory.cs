using UnityEngine;

public class InGameInventory : MonoBehaviour
{
    private bool inventoryOpened = false;
    public GameObject inventoryPage;
    public Transform inventoryListLocation;
    public Transform originalLocation;
    public Transform openInventoryLocation;
    public GameObject textBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void InGameInventoryButtonClicked()
    {
        if(inventoryOpened == true) //close it
        {
            CloseInGameInventory();
        }
        else if(inventoryOpened == false) //if its not open, then open it
        {
            OpenInGameInventory();
        }
    }

    public void OpenInGameInventory()
    {
        inventoryOpened = true;
        inventoryPage.transform.position = openInventoryLocation.position;
        textBox.SetActive(true);
    }
    public void CloseInGameInventory()
    {
        inventoryOpened = false;
        inventoryPage.transform.position = originalLocation.position;
        textBox.SetActive(false);

    }


}
