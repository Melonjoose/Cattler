using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Button : MonoBehaviour // add handlers

{
    public CatUnit catUnit;
    public ActiveAbility assignedSkill;
    public GameObject skillButton;
    public CanvasGroup thisCanvas;
    public Button button;
    private RectTransform buttonRect;
    public Image skillIcon;
    public Icon parentIcon;

    public GameObject cooldownVisual;
    public TextMeshProUGUI cooldownNumber;

    void Awake()
    {
        skillButton = this.gameObject;
        button = skillButton.GetComponent<Button>();
        thisCanvas = skillButton.GetComponent<CanvasGroup>();
        skillIcon = this.transform.Find("Sprite").GetComponent<Image>();
        skillIcon.sprite = null; // change to an X in the future??
        cooldownVisual = skillButton.transform.Find("CooldownVisual").gameObject;
        cooldownNumber = cooldownVisual.transform.Find("CooldownNumber").GetComponent<TextMeshProUGUI>();
        parentIcon = transform.parent.GetComponent<Icon>();
        cooldownVisual.SetActive(false);
        //skillButton.transform.localPosition = Vector3.zero;
        //buttonRect = skillButton.GetComponent<RectTransform>();
        //HideButton();
    }

    public void AssignCat(CatUnit cat)
    {
        catUnit = cat;
    }

    public void AssignSkill(ActiveAbility skill)
    {
        assignedSkill = skill;  

        if (this == parentIcon.skillButton1)
        {
            parentIcon.cooldownDuration1 = skill.cooldown; //store the skill cooldown to parenIcon to be manipulated by any other scripts (eg. faster cooldown etc)
            parentIcon.originalCooldownDuration1 = skill.cooldown; 
        }
        else if (this == parentIcon.skillButton2)
        {
            parentIcon.cooldownDuration2 = skill.cooldown; //store the skill cooldown to parenIcon to be manipulated by any other scripts (eg. faster cooldown etc)
            parentIcon.originalCooldownDuration2 = skill.cooldown; 
        }
    }

    public void RemoveSkill()
    {
        assignedSkill = null;
    }

    public void UpdateIcon(Item item) //handle updating the skill icon based on items
    {
        if (item == null || skillIcon == null) //if no item. or no skillicon. hide this skill icon.
        {
            thisCanvas.alpha = 0f;
            thisCanvas.interactable = false;
            thisCanvas.blocksRaycasts = false;
            skillIcon.sprite = null;
            return;
        }
        //if there is an item. show the icon.
        thisCanvas.alpha = 1f;
        thisCanvas.interactable = true;
        thisCanvas.blocksRaycasts = true;
        Sprite itemIcon = item.runtimeData.template.icon;
        skillIcon.sprite = itemIcon;
    }

    public void UseSkill()
    {
        if(assignedSkill == null)
        {
            Debug.LogWarning("No skill assigned to this button.");
            return;
        }

        if(catUnit == null)
        {
            Debug.LogWarning("No cat unit assigned to this skill button.");
            return;
        }

        Vector3 spawnloc = catUnit.transform.position + assignedSkill.spawnLocationOffset; // Vector3(1,0,0) also works
        GameObject newSkillObject = Instantiate(assignedSkill.AbilityPrefab, spawnloc, transform.rotation); // use rotation, not transform
        ActiveAbility SkillInfo = newSkillObject.GetComponent<ActiveAbility>(); //newskillobject is the new instantiated skill prefab. skillinfo is the activeability script attached to the prefab, which contains the info of the skill.
        CatUnit catInfo = catUnit.GetComponent<CatUnit>(); 
        SkillInfo.catUnit = catInfo; // reference catunit into the skill's activeability script so that the skill can access the cat's info for damage calculation and other purposes.
        //HideButton();
        if (this == parentIcon.skillButton1)
        {
            Cooldown(parentIcon.cooldownDuration1);
        }
        if (this == parentIcon.skillButton2)
        {
            Cooldown(parentIcon.cooldownDuration2);
        }

    }

    void Cooldown(float cooldownTime)
    {
        // Disable button
        button.interactable = false;

        // Set overlay to semi-transparent black
        Image image = cooldownVisual.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0.8f);

        cooldownVisual.SetActive(true);

        // Start cooldown coroutine
        StartCoroutine(CooldownRoutine(cooldownTime, image));
    }

    IEnumerator CooldownRoutine(float cooldownTime, Image image)
    {
        float elapsed = 0f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            float remainingTime = cooldownTime - elapsed;

            // Update countdown text (1 decimal place)
            cooldownNumber.text = remainingTime.ToString("F1");

            // Fade alpha from 0.8 > 0 over time
            float alpha = Mathf.Lerp(0.8f, 0f, elapsed / cooldownTime);
            image.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        // Cooldown complete
        button.interactable = true;
        cooldownVisual.SetActive(false);
    }

}
