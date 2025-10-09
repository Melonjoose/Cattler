using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class CatIconUI : MonoBehaviour
{
    public static CatIconUI instance;

    [System.Serializable]
    public class CatIconSlot
    {
        public Icon icon;
        public Image iconImage;
        public Slider healthBar;
        public int iconIndex = -1;
        public int catIndex = -1;
    }

    [Header("UI Slots")]
    public CatIconSlot[] uiSlots; // assign 5 in Inspector

    [Header("UI Slots")]
    public List<Transform> iconPosition = new List<Transform>();  //ensure that icon is 0 - 4 / left to right.

    [Header("Containers")]
    public GameObject[] catContainer; // assign containers in Inspector 



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
    public void IntializeCatIcon()
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
        Transform iconPositionGRP = GameObject.Find("Position_GRP")?.transform;
        if (iconPositionGRP == null)
        {
            Debug.LogError("iconPositionGRP not assigned in CatPosition!");
            return;
        }
        for (int i = 0; i < 5; i++) //0 - 4
        {
            Transform pos = iconPositionGRP.Find($"PositionIcon{i}");
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


    public void MoveCatIconTo(int catIconIndex, int positionIndex)
    {
        if (catIconIndex < 0 || catIconIndex >= uiSlots.Length)
        {
            Debug.LogWarning($"Invalid CatIcon index: {catIconIndex}");
            return;
        }

        if (positionIndex < 0 || positionIndex >= iconPosition.Count)
        {
            Debug.LogWarning($"Invalid position index: {positionIndex}");
            return;
        }

        RectTransform iconRect = uiSlots[catIconIndex].icon.GetComponent<RectTransform>();
        RectTransform targetRect = iconPosition[positionIndex].GetComponent<RectTransform>();

        if (iconRect != null && targetRect != null)
        {
            iconRect.anchoredPosition = targetRect.anchoredPosition; //  correct way for UI
            uiSlots[catIconIndex].icon.AssignIconIndex(positionIndex);
            uiSlots[catIconIndex].iconIndex = positionIndex;

            Debug.Log($"Icon {catIconIndex} instantly moved to Position {positionIndex}");
        }
        else
        {
            Debug.LogWarning($"Missing RectTransform on icon or position target!");
        }
        Debug.Log($"Before move: {iconRect.anchoredPosition}, After: {targetRect.anchoredPosition}");
    }



}
