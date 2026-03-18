using UnityEngine;

public class CatSkill : MonoBehaviour
{
    public bool isActive; // whether the skill is active or passive
    public string skillDesc;
    public string skillType;
    public float cooldown; // cooldown time for the skill, if applicable
    public float time; //time for cooldown or duration of the skill, depending on the skill type
    public float value; // this can be used for different purposes depending on the skill, such as damage multiplier, heal amount, etc.
    public float speed; // this can be used for skills that affect movement speed or attack speed, etc.

    public GameObject itemObject;
    public CatUnit catUnit; // this is attached to this catUnit.

    private void Start()
    {
        catUnit = GetComponentInParent<CatUnit>();
    }
}
