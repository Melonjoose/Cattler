using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Button : MonoBehaviour // add handlers

{
    public CatUnit catUnit;
    public GameObject skillButton;
    private RectTransform buttonRect;
    public bool isRevealed = false;
    public GameObject slash;
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

    public void RevealSkillButton()
    {
        if (isRevealed == false)
        {
            isRevealed=true;
            skillButton.SetActive(true);
            // Raise button above default position (relative to parent)
            skillButton.transform.localPosition = new Vector3(0f, 62.5f, 0f);
        }
        else
        {
            HideButton();
        }
    }

    public void ClickOnSkill()
    {
        Vector3 spawnloc = catUnit.transform.position + Vector3.right; // Vector3(1,0,0) also works
        GameObject newSlash = Instantiate(slash, spawnloc, transform.rotation); // use rotation, not transform
        Slash SlashInfo = newSlash.GetComponent<Slash>();
        CatUnit catInfo = catUnit.GetComponent<CatUnit>();
        SlashInfo.CatUnit = catInfo;
        HideButton();

    }

    public void HideButton()
    {
        isRevealed = false;
        skillButton.transform.localPosition = new Vector3(0f, 0f, 0f);
        skillButton.gameObject.SetActive(false);
    }

}
