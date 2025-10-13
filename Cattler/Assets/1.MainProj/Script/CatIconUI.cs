using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class CatIconUI : MonoBehaviour
{
    public static CatIconUI instance;

    [System.Serializable]
    public class CatIconSlot
    {
        public int iconIndex = -1;
        public Icon icon;
        public Image iconImage;
        public Slider healthBar;
        public int catIndex = -1;

    }

    [Header("UI Slots")]
    public CatIconSlot[] uiSlots; // assign 5 in Inspector

    [Header("UI Slots Positions")]
    public List<RectTransform> iconPosition = new List<RectTransform>();  //ensure that icon is 0 - 4 / left to right.

    [Header("Containers")]
    public GameObject[] catContainer; // assign containers in Inspector 


    public void OnEnable()
    {
        
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //RefreshIcons();
        InitializeiconPositions();
        IntializeCatIcon();
    }

    private void Update()
    {
        //RefreshIcons(); // left here for simplicity, optimize later if needed
        IntializeCatIcon();
        SyncIconToCatPosition();
    }

    //CatIconUI Logic. (Intializing Syncing)
    public void IntializeCatIcon() // change to be a callfunction
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i >= catContainer.Length) return; // safety check

            ContainerDetector detector = catContainer[i].GetComponent<ContainerDetector>();

            // if the catContainer.cs is not empty.
            if (detector.occupyingCat != null)
            {
                CatUnit cat = detector.occupyingCat;
                if (cat != null)
                {
                    uiSlots[i].icon.linkedCat = cat.catMovement;

                    uiSlots[i].iconImage.sprite = cat.runtimeData.template.icon;
                    uiSlots[i].icon.gameObject.SetActive(true);

                    uiSlots[i].healthBar.gameObject.SetActive(true);
                    uiSlots[i].healthBar.maxValue = cat.runtimeData.maxHealth;
                    uiSlots[i].healthBar.value = cat.runtimeData.currentHealth;

                    uiSlots[i].iconIndex = i;
                    uiSlots[i].catIndex = cat.catMovement.catIndex;

                }
            }
            else
            {
                // No cat in this container → hide icon & healthbar
                uiSlots[i].icon.gameObject.SetActive(false);
                uiSlots[i].healthBar.gameObject.SetActive(false);
            }
        }
    }

    void InitializeiconPositions()
    {
        // 1️⃣ Get the parent group Transform
        Transform iconPositionGRP = GameObject.Find("Position_GRP")?.transform;
        if (iconPositionGRP == null)
        {
            Debug.LogError("Position_GRP not found!");
            return;
        }

        iconPosition.Clear(); // Always good to clear before adding new ones

        // 2️⃣ Loop through child objects
        for (int i = 0; i < 5; i++)
        {
            Transform posTransform = iconPositionGRP.Find($"PositionIcon{i}");
            if (posTransform != null)
            {
                RectTransform posRect = posTransform as RectTransform;
                iconPosition.Add(posRect); // Add RectTransform to the list
                Debug.Log($"Assigned {posRect.name} as index {i}");
            }
            else
            {
                Debug.LogWarning($"PositionIcon{i} not found under {iconPositionGRP.name}");
            }
        }
    }


    void SyncIconToCatPosition()
    {
        // Sync iconIndex to catIndex for each slot if not already matching
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].iconIndex != uiSlots[i].catIndex && uiSlots[i].catIndex != -1)
            {
                uiSlots[i].iconIndex = uiSlots[i].catIndex;
            }
        }
    }
    /*
    public void MoveCatIconTo(int catIconIndex, int positionIndex)
    {
        // safety checks
        if (uiSlots == null || uiSlots.Length == 0)
        {
            Debug.LogWarning("uiSlots not set up");
            return;
        }
        if (positionIndex < 0 || positionIndex >= iconPosition.Count)
        {
            Debug.LogWarning($"Invalid positionIndex {positionIndex}");
            return;
        }

        // 1) find the uiSlot that currently represents the cat with catIndex == catIconIndex
        CatIconSlot foundSlot = null;
        int foundIndexInArray = -1;
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i] != null && uiSlots[i].catIndex == catIconIndex)
            {
                foundSlot = uiSlots[i];
                foundIndexInArray = i;
                break;
            }
        }

        if (foundSlot == null)
        {
            Debug.LogWarning($"No uiSlot found for catIndex {catIconIndex}");
            return;
        }

        // 2) get transforms
        RectTransform movingIconRect = foundSlot.icon?.GetComponent<RectTransform>();
        RectTransform targetRect = iconPosition[positionIndex] as RectTransform;

        if (movingIconRect == null || targetRect == null)
        {
            Debug.LogWarning("Missing RectTransforms on icon or target.");
            return;
        }

        // 3) convert target position into the moving icon's parent local space and set it
        // This works even if the two objects live under different parents/canvases.
        Vector3 worldTarget = targetRect.TransformPoint(Vector3.zero);
        Vector3 localTarget = movingIconRect.parent.InverseTransformPoint(worldTarget);
        //movingIconRect.localPosition = localTarget;
        LeanTween.moveLocal(movingIconRect.gameObject, localTarget, 0.25f).setEase(LeanTweenType.easeInOutQuad);

        // 4) update bookkeeping so the slot knows its new logical position
        // set both the iconIndex (where it visually sits) and catIndex (which cat it represents)
        foundSlot.iconIndex = positionIndex;
        // if you want catIndex to change to the same as iconIndex (rare), uncomment:
        // foundSlot.catIndex = positionIndex;


        Debug.Log($"Moved uiSlot[{foundIndexInArray}] (catIndex {catIconIndex}) -> position {positionIndex}");
    }*/






}
