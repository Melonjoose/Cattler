using System.ComponentModel;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public GameObject skillButtonPH; //to be instantiated and edited using data receive to create a new skill.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    void InitializeSkill()
    {
        //instantiate button @skillbuttonlocation inside of cat catprefab that equip that item with a skill.
        //allocate the skill type based on that item.
        //set the button active to false. hide it.
    }


}
