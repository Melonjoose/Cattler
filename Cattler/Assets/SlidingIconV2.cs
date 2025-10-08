using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingIconV2 : MonoBehaviour
{
    public List<Icon> Icons = new List<Icon>();
    public List<GameObject> Position = new List<GameObject>();
    private CatMovement[] catMovement;

    private void OnEnable()
    {
        //check for all catMovement
        //catMovement.onMove += MoveIconToLocation; //subscribe to onMove // whenonMove is invoked. MoveIconTolocation is played
    }
    private void OnDisable()
    {

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
