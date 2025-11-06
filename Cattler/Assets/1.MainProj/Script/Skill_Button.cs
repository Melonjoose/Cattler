using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Button : MonoBehaviour // add handlers

{
    public CatUnit catUnit;
    public GameObject skillButton;
    private RectTransform buttonRect;

    public GameObject skillPrefab;

    public ActiveAbility activeAbility;
    void Start()
    {
        //skillButton.transform.localPosition = Vector3.zero;
        //buttonRect = skillButton.GetComponent<RectTransform>();
        //HideButton();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseSkill(GameObject skill)
    {
        Vector3 spawnloc = catUnit.transform.position + Vector3.right; // Vector3(1,0,0) also works
        GameObject newSlash = Instantiate(skillPrefab, spawnloc, transform.rotation); // use rotation, not transform
        Slash SlashInfo = newSlash.GetComponent<Slash>();
        CatUnit catInfo = catUnit.GetComponent<CatUnit>();
        SlashInfo.catUnit = catInfo;
        //HideButton();

    }
}
