using Unity.VisualScripting;
using UnityEngine;

public class FastCaster : CatSkill
{
    public Icon icon; //the skill to be edited.
                      // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        catUnit = GetComponentInParent<CatUnit>();
        icon = catUnit.icon;
        DecreaseCoolDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float originalCDduration1;
    public float originalCDduration2;

    //passive that is applied when this skill is attached to the catUnit.
    public void DecreaseCoolDown()//reduce both cooldown by 20%
    {
        if (icon != null)
        {
            originalCDduration1 = icon.cooldownDuration1;
            originalCDduration2 = icon.cooldownDuration2;

            icon.cooldownDuration1 = (icon.cooldownDuration1 / 100) * 80; 
            icon.cooldownDuration2 = (icon.cooldownDuration2 / 100) * 80; 
        }
    }

    public void ResetCoolDown()
    {
        if(icon != null)
        {
            icon.cooldownDuration1 = originalCDduration1;
            icon.cooldownDuration2 = originalCDduration2;
        }
    }
}
