using Spine;
using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CatRoamLobby : MonoBehaviour
{
    //this is to handle cats that are added into team list , hence showing up in the lobby. cats in the lobby are free roaming within the bound box.(a gamobject with rect transform) cats will walk and idle randomly.
    public static CatRoamLobby instance; // Singleton instance for easy access
    public List<GameObject> catInLobby = new List<GameObject>();
    public GameObject catLobbyPrefab; // Prefab for the cat unit to be instantiated in the lobby
    public GameObject boundBox;
    public GameObject lobbyCatGroup;
    public GameObject battleDoor; //all cat will target this and move to it when movetodoor is called.
    public void Start()
    {
        instance = this; // Set the singleton instance
    }

    void Update()
    {
        ReorderCats();
    }

    public void AddCatToLobby(CatUnit cat)
    {
        
        
        Debug.Log("Adding cat to lobby");
        GameObject newCat = Instantiate(catLobbyPrefab, GetRandomPositionWithinBounds(), Quaternion.identity); //create prefab instance of the cat in the lobby
        newCat.name = $"{cat.runtimeData.template.itemName}_Lobby";
        newCat.transform.SetParent(lobbyCatGroup.transform); // ensure its in the lobby layer.
        RectTransform catRect = newCat.GetComponent<RectTransform>(); //get the rect transform of the cat instance
        catRect.localScale = new Vector3(0.12f, 0.12f, 0.12f); //scale down the cat instance to fit the lobby better. adjust as needed.
        catInLobby.Add(newCat); //add to list
        CatUnit catUnit = newCat.GetComponent<CatUnit>(); //get the cat unit component from the prefab instance

        catUnit.AssignCat(cat.runtimeData); //assign the cat's runtime data to the cat unit in the lobby so it can display the correct appearance and stats. //working
        LobbyCat lobbyCat = newCat.GetComponent<LobbyCat>();
        lobbyCat.skinName = cat.runtimeData.template.skinName;

        catUnit.catLobby = newCat;
        catUnit.weaponL = cat.weaponL;
        catUnit.weaponR = cat.weaponR;
        catUnit.hat = cat.hat;

        cat.catLobby = newCat;

        if (catUnit.hat != null)
        {
            EquipItem(catUnit.hat, catUnit, "T_HatSlot");
        }
        if (catUnit.weaponL != null)
        {
            EquipItem(catUnit.weaponL, catUnit, "L_WeaponSlot");
        }
        if (catUnit.weaponR != null)
        {
            EquipItem(catUnit.weaponR, catUnit, "R_WeaponSlot");
        }

        InventoryIcon icon = cat.inventoryIcon;
        catUnit.inventoryIcon = icon;

        CatUnit catIconCS = catUnit.inventoryIcon.GetComponent<CatUnit>();
        catIconCS.catLobby = newCat;

    }

    public void RemoveCatFromLobby(CatUnit cat)
    {
        //cat.catLobby.SetActive(false); //deactivate the cat gameobject in the lobby when removed from team list. you can also choose to destroy it if you prefer.
        catInLobby.Remove(cat.catLobby);
        Destroy(cat.catLobby);
        cat.catLobby = null;
    }

    public Vector3 GetRandomPositionWithinBounds()
    {
        Vector3[] corners = new Vector3[4];
        boundBox.GetComponent<RectTransform>().GetWorldCorners(corners);

        float x = Random.Range(corners[0].x, corners[2].x);
        float y = Random.Range(corners[0].y, corners[2].y);

        return new Vector3(x, y, 0);

    }

    public void DisableAllLobbyCat()
    {
        foreach (GameObject cat in catInLobby)
        {
            cat.gameObject.SetActive(false);
        }
    }

    public void EnableAllLobbyCat()
    {
        foreach (GameObject cat in catInLobby)
        {
            cat.gameObject.SetActive(true);
            cat.transform.position = GetRandomPositionWithinBounds();
        }
    }

    void EquipItem(Item item, CatUnit LobbyCat, string slotName)  //Equip item visuals on world cat. if no item, hide the visuals.
    {

        if (item == null || item.runtimeData?.template?.icon == null)
        {
            Debug.LogWarning("Invalid item or missing icon.");
            return;
        }
        Debug.Log("EQUIPITEM1.2");
        Sprite newIcon = item.runtimeData.template.icon; //change icon visual using template.

        // Hat Slot
        if (slotName == "T_HatSlot")
        {
            Transform hatSlot = LobbyCat.transform.Find("T_HatSlot");
            if (hatSlot != null)
            {
                Transform hat = hatSlot.Find("Hat");
                if (LobbyCat.hat != null)
                {
                    hat.gameObject.SetActive(true);
                    Image sr = hat.GetComponent<Image>();
                    sr.enabled = true;
                    sr.sprite = item.runtimeData.template.icon;
                }
                else
                {
                    Debug.Log("worldCat has no hat");
                    hat.gameObject.SetActive(false);
                    Image sr = hat.GetComponent<Image>();
                    sr.sprite = null;
                    sr.enabled = false;
                }
            }
        }

        // Left Weapon Slot
        if (slotName == "L_WeaponSlot") //checker for what slot to update.
        {

            Transform leftWeaponSlot = LobbyCat.transform.Find("L_WeaponSlot");
            if (leftWeaponSlot != null)
            {

                Transform weapon = leftWeaponSlot.Find("Weapon");
                Image sr = weapon.GetComponent<Image>();

                if (LobbyCat.weaponL != null) // weapon equipped
                {

                    weapon.gameObject.SetActive(true);
                    sr.enabled = true;
                    sr.sprite = LobbyCat.weaponL.runtimeData.template.icon;
                }
                else // no weapon equipped
                {
                    Debug.Log("worldCat has no left weapon");
                    sr.sprite = null;
                    sr.enabled = false;
                    weapon.gameObject.SetActive(false);
                }
            }
        }


        // Right Weapon Slot
        if (slotName == "R_WeaponSlot")
        {
            Transform rightWeaponSlot = LobbyCat.transform.Find("R_WeaponSlot");
            if (rightWeaponSlot != null)
            {
                Transform weapon = rightWeaponSlot.Find("Weapon");
                if (LobbyCat.weaponR != null)
                {
                    weapon.gameObject.SetActive(true);
                    Image sr = weapon.GetComponent<Image>();
                    sr.enabled = true;
                    sr.sprite = item.runtimeData.template.icon;
                }
                else
                {
                    Debug.Log("worldCat has no right weapon");
                    weapon.gameObject.SetActive(false);
                    Image sr = weapon.GetComponent<Image>();
                    sr.sprite = null;
                    sr.enabled = false;
                }
            }
        }
    }

    void ReorderCats()
    {
        catInLobby.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

        // Apply sibling order based on sorted list
        for (int i = 0; i < catInLobby.Count; i++)
        {
            catInLobby[i].transform.SetSiblingIndex(i);
        }
    }

    public void AllCatsMoveToBattleDoor()
    {
        foreach (GameObject cat in catInLobby)
        {
            LobbyCat lobbyCat = cat.GetComponent<LobbyCat>();
            lobbyCat.MoveCatTo(battleDoor.transform.position);
        }
    }

}
