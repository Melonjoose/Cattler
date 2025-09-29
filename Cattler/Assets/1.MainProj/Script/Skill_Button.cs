using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Button : MonoBehaviour // add handlers

{
    public GameObject cat;
    public GameObject skillButton;
    private RectTransform buttonRect;
    public bool isRevealed = false;
    public GameObject slash;
    void Start()
    {
        skillButton.transform.localPosition = Vector3.zero;
        buttonRect = skillButton.GetComponent<RectTransform>();
        HideButton();
    }

    // Update is called once per frame
    void Update()
    {
        DetectCat();
    }

    public void CatIconOnClicked()
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
        Vector3 spawnloc = cat.transform.position + Vector3.right; // Vector3(1,0,0) also works
        GameObject newSlash = Instantiate(slash, spawnloc, transform.rotation); // use rotation, not transform
        Slash SlashInfo = newSlash.GetComponent<Slash>();
        CatUnit catInfo = cat.GetComponent<CatUnit>();
        SlashInfo.CatUnit = catInfo;
        HideButton();

    }


    public void HideButton()
    {
        isRevealed = false;
        skillButton.transform.localPosition = new Vector3(0f, 0f, 0f);
        skillButton.gameObject.SetActive(false);
    }

    void DetectCat()
    {
        if(cat == null)
        {
            cat = TeamManager.instance.teamCats[0];
        }
    }
}
