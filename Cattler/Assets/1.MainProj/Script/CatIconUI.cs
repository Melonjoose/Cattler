using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CatIconUI : MonoBehaviour
{
    public static CatIconUI instance;

    [System.Serializable]
    public class CatIconSlot
    {
        public int iconIndex = -1;
        public int initialIconIndex = -1; // to reset position
        public Icon icon;
        public Image iconImage;
        public Slider healthBar;
        public CatUnit unit;
        public bool isDead = false;
    }

    [Header("UI Slots")]
    public CatIconSlot[] uiSlots; // assign 5 in Inspector

    [Header("UI Slots Positions")]
    public List<RectTransform> iconPosition = new List<RectTransform>();  //ensure that icon is 0 - 4 / left to right.

    public Sprite deathIcon;

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
        //Debug.Log($"{i} and {cat}");
        if (cat != null)
        {
            uiSlots[i].iconIndex = i;
            uiSlots[i].initialIconIndex = i;
            uiSlots[i].icon.gameObject.SetActive(true);

            uiSlots[i].unit = cat;
            uiSlots[i].iconImage.sprite = cat.runtimeData.template.icon;

            uiSlots[i].healthBar.gameObject.SetActive(true);
            uiSlots[i].healthBar.maxValue = cat.runtimeData.maxHealth;
            uiSlots[i].healthBar.value = cat.runtimeData.currentHealth;

            var thisIconPosition = uiSlots[i].icon.gameObject.transform.position;
            var positionSlotToSnap = iconPosition[i].transform.position;

            thisIconPosition = positionSlotToSnap;

            cat.catIconSlot = uiSlots[i];

            Button catUISlotbutton = uiSlots[i].icon.GetComponent<Button>();  //enable button ineractions
            catUISlotbutton.interactable = true;

            cat.onHealthChanged += (current, max) => UpdateIconHealthUI(i, current, max);
        }
        else
        {
            // No cat in this container > hide icon & healthbar
            uiSlots[i].icon.gameObject.SetActive(false);
            uiSlots[i].healthBar.gameObject.SetActive(false);

        }

        Icon thisIcon = uiSlots[i].icon;
        
        thisIcon.skillButton1.UpdateIcon(cat.weaponL);
        thisIcon.skillButton2.UpdateIcon(cat.weaponR);

        thisIcon.catUnit = cat; //make icon reference the catUnit.
        cat.icon = thisIcon; // make the catUnit reference the icon


        if (cat.weaponL != null || cat.weaponR != null || cat.hat != null)
        {
            Debug.Log("Linking skill!");
            // Show the skill button
            thisIcon.ShowButton();

            // Find the child button correctly
            Skill_Button skillButton1 = thisIcon.skillButton1;
            if (skillButton1 != null)
            {
                skillButton1.AssignCat(cat);
                if (cat.weaponL != null)
                {
                    skillButton1.AssignSkill(cat.weaponL.runtimeData.template.skill);
                    skillButton1.UpdateIcon(cat.weaponL);
                }
                else
                {
                    Debug.Log("No left weapon found for skill assignment.");
                }
            }

            Skill_Button skillButton2 = thisIcon.skillButton2;
            if (skillButton2 != null)
            {
                skillButton2.AssignCat(cat);
                if (cat.weaponR != null)
                {
                    skillButton2.AssignSkill(cat.weaponR.runtimeData.template.skill);
                    skillButton2.UpdateIcon(cat.weaponR);
                }
                else
                {
                    Debug.Log("No right weapon found for skill assignment.");
                }
            }
        }
        if(cat.weaponL == null) { thisIcon.cooldownDuration1 = 0; thisIcon.originalCooldownDuration1 = 0; }
        if(cat.weaponR == null) { thisIcon.cooldownDuration2 = 0; thisIcon.originalCooldownDuration2 = 0; }         
    }

    public void UnlinkCatFromIcon(CatUnit cat) //the catUnit is to determine what icon should be unlinked. to select the correct icon. meaning without cat, this script doesn't know which icon to unlink.
    {
        Debug.Log("UNLINKING PLAYING");

        CatUnit catToBeRemoved = cat;

        // Find the slot that contains this cat
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].unit == catToBeRemoved)
            {
                Debug.Log("UNLINKING PLAYING2");
                //unlinks the cat icon from the slot
                // Move the icon back to its original position
                var thisIconPosition = uiSlots[i].icon.gameObject.transform.position;
                var positionSlotToSnap = iconPosition[uiSlots[i].initialIconIndex].transform.position; //make sure that eg. slot 0 is move to it's original slot of slot 0.

                thisIconPosition = positionSlotToSnap;


                //unlink skill buttons .. do first before removing cat from slot.
                Icon thisIcon = uiSlots[i].icon;
                thisIcon.skillButton1.RemoveSkill();
                thisIcon.skillButton1.UpdateIcon(catToBeRemoved.weaponL);
                thisIcon.skillButton2.RemoveSkill();
                thisIcon.skillButton2.UpdateIcon(catToBeRemoved.weaponR);

                //icon return to original state
                { thisIcon.cooldownDuration1 = 0; thisIcon.originalCooldownDuration1 = 0; }
                { thisIcon.cooldownDuration2 = 0; thisIcon.originalCooldownDuration2 = 0; }

                thisIcon.catUnit = null; //remove cat and make it null
                thisIcon.gameObject.SetActive(false);

                // Unlink the cat from this slot
                uiSlots[i].iconIndex = -1;
                uiSlots[i].unit = null;

                uiSlots[i].icon.gameObject.SetActive(false);
                uiSlots[i].healthBar.gameObject.SetActive(false);

                Debug.Log($"Unlinked cat from icon slot {i}");
                return;
            }
        }
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

       // Debug.Log($"Moving {cat.name}'s icon to slot {newIconIndex}");

        LeanTween.move(movingIcon.gameObject, targetSlot.position, 0.4f)
            .setEase(LeanTweenType.easeInOutQuad);
    }


    public void SetIconToDead(CatIconSlot catUISlot)
    {
        catUISlot.iconImage.sprite = deathIcon;
        Button catUISlotbutton = catUISlot.icon.GetComponent<Button>();
        catUISlotbutton.interactable = false;
        catUISlot.healthBar.gameObject.SetActive(false);
        catUISlot.isDead = true;
        
        //disable all skills

    }
    public void UnlinkSkill(CatUnit cat)
    {
               // Find the slot that contains this cat
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].unit == cat)
            {
                Icon thisIcon = uiSlots[i].icon;
                thisIcon.skillButton1.RemoveSkill();
                thisIcon.skillButton1.UpdateIcon(cat.weaponL);
                thisIcon.skillButton2.RemoveSkill();
                thisIcon.skillButton2.UpdateIcon(cat.weaponR);
            }
        }
    }
    public void DisableAllSkills()
    {
        foreach (var slot in uiSlots)
        {
            if (slot.unit != null)
            {
                Icon thisIcon = slot.icon;
                thisIcon.skillButton1.button.interactable = false;
                thisIcon.skillButton2.button.interactable = false;
            }
        }
    }
    public void EnableAllSkills()
    {
        foreach (var slot in uiSlots)
        {
            if (slot.unit != null)
            {
                Icon thisIcon = slot.icon;
                thisIcon.skillButton1.button.interactable = true;
                thisIcon.skillButton2.button.interactable = true;
            }
        }
    }

    public void ResetIconToDefault(CatIconSlot catUISlot)
    {
        catUISlot.iconImage.sprite = null;
        Button catUISlotbutton = catUISlot.icon.GetComponent<Button>();
        catUISlotbutton.interactable = false;
        catUISlot.unit = null;
        catUISlot.healthBar.gameObject.SetActive(false);
        catUISlot.isDead = false;
        catUISlot.icon.gameObject.SetActive(false); //hide it 
    }

    public void ResetAllIconToInitialIndex()
    {
        // Loop through all uiSlots and reset their positions to their intial index
        for (int i = 0; i < uiSlots.Length; i++)
        {
            var slot = uiSlots[i];
            if (slot.unit != null)
            {
                var targetSlot = iconPosition[i].transform;
                LeanTween.move(slot.icon.gameObject, targetSlot.position, 0.4f)
                    .setEase(LeanTweenType.easeInOutQuad);

                if(slot.isDead == true)
                {
                    ResetIconToDefault(slot);
                }
            }
        }
    }

}
