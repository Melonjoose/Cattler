using System.Collections.Generic;
using TMPro;
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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeiconPositions();
        IntializeCatIcon();
    }

    private void Update()
    {
        //IntializeCatIcon();
        //SyncIconToCatPosition();
    }

    //CatIconUI Logic. (Intializing Syncing)
    public void IntializeCatIcon() // change to be a callfunction
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            { 
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

    public void UpdateIcons(int i, CatUnit cat) //This Icon 
    {
        if (i >= catContainer.Length) return; // safety check

        // if the catContainer.cs is not empty.
        if (cat != null)
        {
            uiSlots[i].iconIndex = i;

            uiSlots[i].icon.gameObject.SetActive(true);
            
            uiSlots[i].icon.linkedCat = cat.catMovement;
            uiSlots[i].iconImage.sprite = cat.runtimeData.template.icon;

            uiSlots[i].healthBar.gameObject.SetActive(true);
            uiSlots[i].healthBar.maxValue = cat.runtimeData.maxHealth;
            uiSlots[i].healthBar.value = cat.runtimeData.currentHealth;

            uiSlots[i].catIndex = cat.catMovement.catIndex;


            uiSlots[i].icon.transform.position = iconPosition[cat.catMovement.catIndex].transform.position;
            Debug.Log($"update Icon{i}");
        }
        else
        {
            // No cat in this container → hide icon & healthbar
            uiSlots[i].icon.gameObject.SetActive(false);
            uiSlots[i].healthBar.gameObject.SetActive(false);
            Debug.Log("NocattoUpdate");
        }
    }

}
