using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Icon : MonoBehaviour
{
    public bool isRevealed = false;
    public Skill_Button skillButton1, skillButton2;
    public GameObject cooldown1, cooldown2;
    public float cooldownDuration1, cooldownDuration2; //cooldown durations for skills

    public Icon thisIcon;

    private void Awake()
    {
        thisIcon = this;
        skillButton1 = transform.Find("SkillButton1").GetComponent<Skill_Button>();
        skillButton2 = transform.Find("SkillButton2").GetComponent<Skill_Button>();
        cooldown1 = skillButton1.transform.Find("CooldownVisual1").gameObject;
        cooldown2 = skillButton2.transform.Find("CooldownVisual2").gameObject;

        cooldown1.SetActive(false);
        cooldown2.SetActive(false);

        thisIcon.HideButton();
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
            ShowButton();
        }
        else
        {
            HideButton();
        }
    }

    public void ShowButton()
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
    public void HideButton()
    {
        LeanTween.moveLocal(skillButton1.gameObject, new Vector3(-16f, 0f, 0.1f), 0.2f)
            .setOnComplete(() => skillButton1.gameObject.SetActive(false));

        LeanTween.moveLocal(skillButton2.gameObject, new Vector3(15f, 0f, 0.1f), 0.2f)
            .setOnComplete(() => skillButton2.gameObject.SetActive(false));

        isRevealed = false;
    }

    public IEnumerator CooldownRoutine(float cooldownTime, Button button, GameObject cooldownVisual)
    {
        float elapsed = 0f;
        cooldownVisual.SetActive(true);

        TextMeshProUGUI cooldownNumber = cooldownVisual.transform.Find("CooldownNumber").GetComponent<TextMeshProUGUI>();
        Image image = cooldownVisual.GetComponent<Image>();

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            float remainingTime = cooldownTime - elapsed;

            // Update countdown text
            cooldownNumber.text = remainingTime.ToString("F1");

            // Fade alpha
            float alpha = Mathf.Lerp(0.8f, 0f, elapsed / cooldownTime);
            image.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        // Cooldown complete
        button.interactable = true;
        cooldownVisual.SetActive(false);
    }
}
