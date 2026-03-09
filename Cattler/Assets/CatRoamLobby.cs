using Spine;
using Spine.Unity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatRoamLobby : MonoBehaviour
{
    //this is to handle cats that are added into team list , hence showing up in the lobby. cats in the lobby are free roaming within the bound box.(a gamobject with rect transform) cats will walk and idle randomly.
    public static CatRoamLobby instance; // Singleton instance for easy access
    public List<GameObject> catInLobby = new List<GameObject>();
    public GameObject catLobbyPrefab; // Prefab for the cat unit to be instantiated in the lobby
    public GameObject boundBox;
    public void Start()
    {
        instance = this; // Set the singleton instance
    }

    public void AddCatToLobby(CatUnit cat)
    {
        Debug.Log("Adding cat to lobby");
        GameObject newCat = Instantiate(catLobbyPrefab, GetRandomPositionWithinBounds(), Quaternion.identity); //create prefab instance of the cat in the lobby
        newCat.name = $"{cat.runtimeData.template.itemName}_Lobby";
        newCat.transform.SetParent(this.transform); // ensure its in the lobby layer.
        RectTransform catRect = newCat.GetComponent<RectTransform>(); //get the rect transform of the cat instance
        catRect.localScale = new Vector3(0.12f, 0.12f, 0.12f); //scale down the cat instance to fit the lobby better. adjust as needed.
        catInLobby.Add(newCat); //add to list
        CatUnit catUnit = newCat.GetComponent<CatUnit>(); //get the cat unit component from the prefab instance
        catUnit.AssignCat(cat.runtimeData); //assign the cat's runtime data to the cat unit in the lobby so it can display the correct appearance and stats. //working
    }

    public void RemoveCatFromLobby(CatUnit cat)
    {
        cat.catGO.SetActive(false); //deactivate the cat gameobject in the lobby when removed from team list. you can also choose to destroy it if you prefer.
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
        }
    }

}
