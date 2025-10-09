using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingIconV2 : MonoBehaviour
{
    //PositionManagerV1.instance.containers[0 - 4] is basically containers with their index 0 - 4 / left to right.
    private Transform iconPositionGRP;
    public List<Transform> iconPosition = new List<Transform>();  //ensure that icon is 0 - 4 / left to right.
    
    public List<Icon> Icons = new List<Icon>();  //to get data from catUnit. // set icon position to iconpositions.transform
    private CatUnit[] catUnit; //Can accessCatmovement with this.  set data to icon

    private void OnEnable()
    {
        InitializeiconPositions();
        TeamManager.instance.onTeamAdd += InitializeCatIcon;
    }
    private void OnDisable()
    {

    }

    public void Update()
    {
        //catMovement detector. get data from cat. set icon to cat index / cat datas like sprites and skills?
    }

    void InitializeiconPositions()
    {
        iconPositionGRP = GameObject.Find("Position_GRP")?.transform;
        if (iconPositionGRP == null)
        {
            Debug.LogError("iconPositionGRP not assigned in CatPosition!");
            return;
        }
        for (int i = 0; i < 5; i++) //0 - 4
        {
            Transform pos = iconPositionGRP.Find($"Position{i}_Icon");
            if (pos != null)
            {
                iconPosition.Add(pos); //
                //Debug.Log($"Assigned {pos.name} as index {i}");
            }
            else
            {
                Debug.LogWarning($"Position{i} not found under {iconPositionGRP.name}");
            }
        }

    }

    void InitializeCatIcon()
    {
        Debug.Log("new icon added.");
    }

    public void MoveIconToLocation(int locationindex)
    {
        Debug.Log($"Icon move to index {locationindex}");
    }


    //Add UIPositions into Position list. this positions are where icons can go to.

    //Add CatIcons[i] to Icons List.
    //hide/disable all caticons[i] at the beginning.


    //Need to detect containers[i] if there are cat in the world.
    //if there are cats in the container,
    //enable the caticon[i]. search for catIcon's OccupyingCat(catunit)
    //Change catIcon's Image.sprite to Catunit.template.sprite
    //
}
