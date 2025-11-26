using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Button : MonoBehaviour // add handlers

{
    public CatUnit catUnit;
    public ActiveAbility assignedSkill;
    public GameObject skillButton;
    private RectTransform buttonRect;
    public Image skillIcon;

    void Awake()
    {
        skillButton = this.gameObject;
        skillIcon = this.transform.Find("Sprite").GetComponent<Image>();
        skillIcon.sprite = null; // change to an X in the future??

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

    }

    public void UpdateIcon(Item item)
    {
        if(item == null)
        {
            skillIcon.sprite = null;
            Debug.LogWarning("No item provided to update icon.");
            return;
        }
        if(skillIcon == null)
        {
            Debug.LogWarning("Skill icon Image component not found.");
            return;
        } 
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

        Vector3 spawnloc = catUnit.transform.position + Vector3.right; // Vector3(1,0,0) also works
        GameObject newSlash = Instantiate(assignedSkill.AbilityPrefab, spawnloc, transform.rotation); // use rotation, not transform
        Slash SlashInfo = newSlash.GetComponent<Slash>();
        CatUnit catInfo = catUnit.GetComponent<CatUnit>();
        SlashInfo.catUnit = catInfo;
        //HideButton();

    }
}
