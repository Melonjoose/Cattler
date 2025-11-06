using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        public CatUnit unit;
    }

    [Header("UI Slots")]
    public CatIconSlot[] uiSlots; // assign 5 in Inspector

    [Header("UI Slots Positions")]
    public List<RectTransform> iconPosition = new List<RectTransform>();  //ensure that icon is 0 - 4 / left to right.

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
                //Debug.Log($"Assigned {posRect.name} as index {i}");
            }
            else
            {
                Debug.LogWarning($"PositionIcon{i} not found under {iconPositionGRP.name}");
            }
        }
    }

    public void LinkCatToIcon(int i, CatUnit cat) //This Icon 
    {
       Debug.Log($"{i} and {cat}");
       if (cat != null)
        {
            uiSlots[i].iconIndex = i;
            uiSlots[i].icon.gameObject.SetActive(true);

            uiSlots[i].unit = cat;
            uiSlots[i].iconImage.sprite = cat.runtimeData.template.icon;

            uiSlots[i].healthBar.gameObject.SetActive(true);
            uiSlots[i].healthBar.maxValue = cat.runtimeData.maxHealth;
            uiSlots[i].healthBar.value = cat.runtimeData.currentHealth;

            var thisIconPosition = uiSlots[i].icon.gameObject.transform.position;
            var positionSlotToSnap = iconPosition[i].transform.position;

            thisIconPosition = positionSlotToSnap;

            cat.onHealthChanged += (current, max) => UpdateIconHealthUI(i , current, max);
        }
        else
        {
            // No cat in this container > hide icon & healthbar
            uiSlots[i].icon.gameObject.SetActive(false);
            uiSlots[i].healthBar.gameObject.SetActive(false);

        }
    }

    public void UnlinkCatFromIcon(CatUnit cat , int i)
    {

    }

    private void UpdateIconHealthUI(int i , float currentHealth, float maxHealth)
    {
        // Find the correct slot (if needed) and update the health bar
        // Example assumes you're updating the currently linked slot
        uiSlots[i].healthBar.maxValue = maxHealth;
        uiSlots[i].healthBar.value = currentHealth;
    }


    public void MoveIcon(CatUnit cat, int newIconIndex)
    {
        // Find the icon whose CatUnit matches this cat
        var movingIcon = uiSlots.FirstOrDefault(slot => slot.unit == cat)?.icon;
        if (movingIcon == null)
        {
            Debug.LogWarning($"No icon found for {cat.name}");
            return;
        }

        // Get target slot
        var targetSlot = iconPosition[newIconIndex].transform;

        Debug.Log($"Moving {cat.name}'s icon to slot {newIconIndex}");

        LeanTween.move(movingIcon.gameObject, targetSlot.position, 0.4f)
            .setEase(LeanTweenType.easeInOutQuad);
    }

}
