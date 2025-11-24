using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Button : MonoBehaviour // add handlers

{
    public CatUnit catUnit;
    public ActiveAbility assignedSkill;
    public GameObject skillButton;
    private RectTransform buttonRect;

    void Start()
    {
        skillButton = this.gameObject;

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
