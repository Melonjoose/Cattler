using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CatPositionManager : MonoBehaviour
{
    //public List<Transform> worldPos = new List<Transform>(); // world positions for cats
    //public List<CatPosition> catPositions = new List<CatPosition>();
    public GameObject worldPos1,worldPos2, worldPos3, worldPos4, worldPos5;

    public GameObject catContainer1;
    public GameObject catContainer2;
    public GameObject catContainer3;
    public GameObject catContainer4;
    public GameObject catContainer5;

   
    public DraggableIcon icon1;
    public int iconIndex1 = 0;
    public int catIndex1 = 0;
    public DraggableIcon icon2;
    public int iconIndex2 = 0;
    public int catIndex2 = 0;
    public DraggableIcon icon3;
    public int iconIndex3 = 0;
    public int catIndex3 = 0;
    public DraggableIcon icon4;
    public int iconIndex4 = 0;
    public int catIndex4 = 0;
    public DraggableIcon icon5;
    public int iconIndex5 = 0;
    public int catIndex5 = 0;

    public bool catInContainer1 = false;
    public bool catInContainer2 = false;
    public bool catInContainer3 = false;
    public bool catInContainer4 = false;
    public bool catInContainer5 = false;

    public GameObject worldPositionGRP; // Parent object containing world position transforms

    public static CatPositionManager instance;

    private void Start()
    {
        instance = this;
        AssignWorldPositions();
        UpdateManager();
    }

    void AssignWorldPositions() 
    {
        //Under CatPoints_GRP gameobject, assign children transform called pos1, pos 2, pos 3, etc.
        //worldPos.Clear();

        //foreach (Transform child in worldPositionGRP.transform)
        //{
        //   if (child.name.StartsWith("Position"))
        //       worldPos.Add(child);
        //}


    }

    public void UpdateManager()
    {
        UpdateIconIndices();
        UpdateCatIndices();
    }

    public void UpdateIconIndices()
    {
        //refer to slidingicon.cs icons list. then add exact list to this list
        iconIndex1 = icon1.iconIndex;
        iconIndex2 = icon2.iconIndex;
        iconIndex3 = icon3.iconIndex;
        iconIndex4 = icon4.iconIndex;
        iconIndex5 = icon5.iconIndex;
    }

    public void UpdateCatIndices()
    {
        catIndex1 = iconIndex1;
        catIndex2 = iconIndex2;
        catIndex3 = iconIndex3;
        catIndex4 = iconIndex4;
        catIndex5 = iconIndex5;

        if(catContainer1.GetComponentInChildren<CatPosition>() != null)
        {
            catContainer1.GetComponentInChildren<CatPosition>().AssignCatIndex(catIndex1); //get catposition script from catcontainer1's child and run AssignCatIndex function
            catContainer1.GetComponentInChildren<CatPosition>().AssignWorldPositions();
            catInContainer1 = true;
        }

        if(catContainer2.GetComponentInChildren<CatPosition>() != null)     
        {
            catContainer2.GetComponentInChildren<CatPosition>().AssignCatIndex(catIndex2);
            catContainer2.GetComponentInChildren<CatPosition>().AssignWorldPositions();
            catInContainer2 = true;
        }

        if (catContainer3.GetComponentInChildren<CatPosition>() != null)
        {
            catContainer3.GetComponentInChildren<CatPosition>().AssignCatIndex(catIndex3);
            catContainer3.GetComponentInChildren<CatPosition>().AssignWorldPositions();
            catInContainer3 = true;
        }

        if (catContainer4.GetComponentInChildren<CatPosition>() != null)
        {
            catContainer4.GetComponentInChildren<CatPosition>().AssignCatIndex(catIndex4);
            catContainer4.GetComponentInChildren<CatPosition>().AssignWorldPositions();
            catInContainer4 = true;
        }

        if (catContainer5.GetComponentInChildren<CatPosition>() != null)
        {
            catContainer5.GetComponentInChildren<CatPosition>().AssignCatIndex(catIndex5);
            catContainer5.GetComponentInChildren<CatPosition>().AssignWorldPositions();
            catInContainer5 = true;
        }

    }

 
}
