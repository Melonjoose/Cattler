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
    public Button button;
    private RectTransform buttonRect;
    public Image skillIcon;

    public GameObject cooldownVisual;
    public TextMeshProUGUI cooldownNumber;

    void Awake()
    {
        skillButton = this.gameObject;
        button = skillButton.GetComponent<Button>();
        skillIcon = this.transform.Find("Sprite").GetComponent<Image>();
        skillIcon.sprite = null; // change to an X in the future??


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
        GameObject newSkillObject = Instantiate(assignedSkill.AbilityPrefab, spawnloc, transform.rotation); // use rotation, not transform
        ActiveAbility SkillInfo = newSkillObject.GetComponent<ActiveAbility>();
        CatUnit catInfo = catUnit.GetComponent<CatUnit>();
        SkillInfo.catUnit = catInfo;
        //HideButton();
        Cooldown(assignedSkill.cooldown);
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
