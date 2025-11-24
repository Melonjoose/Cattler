using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class Icon : MonoBehaviour
{
    public bool isRevealed = false;
    public Skill_Button skillButton1, skillButton2;

    public Icon thisIcon;

    private void Awake()
    {
        thisIcon = this;
        skillButton1 = transform.Find("SkillButton1").GetComponent<Skill_Button>();
        skillButton2 = transform.Find("SkillButton2").GetComponent<Skill_Button>();
        thisIcon.HideButton();
    }

    public void InitializeSkills()  //based on what item the cat has, initialize the skills accordingly. //maybe I might shift this to TeamManager later.
    {

    }

    public void IconButtonClicked(Icon icon)
    {
        if(icon != thisIcon)
        {
            thisIcon.HideButton();  //does this hide all other icons when the current attached icon is clicked?
            return;
        }

        if (isRevealed == false)
        {
            isRevealed = true;

            skillButton1.gameObject.SetActive(true);
            skillButton2.gameObject.SetActive(true);
            // Raise button above default position (relative to parent)
            //skillButton1.transform.localPosition = new Vector3(0f, 55f, 0f);
            //skillButton2.transform.localPosition = new Vector3(0f, 100f, 0f);
            LeanTween.moveLocal(skillButton1.gameObject, new Vector3(-16f, 48f, 0.1f), 0.2f);
            LeanTween.moveLocal(skillButton2.gameObject, new Vector3(15f, 48f, 0.1f), 0.2f);
        }
        else
        {
            HideButton();
        }
    }
    public void HideButton()
    {
        LeanTween.moveLocal(skillButton1.gameObject, new Vector3(-16f, 0f, 0.1f), 0.2f)
            .setOnComplete(() => skillButton1.gameObject.SetActive(false));

        LeanTween.moveLocal(skillButton2.gameObject, new Vector3(15f, 0f, 0.1f), 0.2f)
            .setOnComplete(() => skillButton2.gameObject.SetActive(false));

        isRevealed = false;
    }


}
