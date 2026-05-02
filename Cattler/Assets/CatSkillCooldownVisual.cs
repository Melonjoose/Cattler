using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatSkillCooldownVisual : MonoBehaviour
{
    public Icon icon;
    public TextMeshProUGUI cooldownValueText;
    public float maxcooldownValue;
    public float time;
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (icon.catUnit.skill != null)
        {
            time = icon.catUnit.skill.time;
            slider.value = time;
            cooldownValueText.text = time.ToString("F1");
        }
    }

    public void IntializeCDVisual()
    {
        if (icon.catUnit.skill != null)
        {
            slider.gameObject.SetActive(true);
            cooldownValueText.gameObject.SetActive(true);
            maxcooldownValue = icon.catUnit.skill.cooldown;
            slider.maxValue = maxcooldownValue;
            slider.minValue = 0;
        }
        else
        {
            slider.gameObject.SetActive(false);
            cooldownValueText.gameObject.SetActive(false);
        }
    }
}
