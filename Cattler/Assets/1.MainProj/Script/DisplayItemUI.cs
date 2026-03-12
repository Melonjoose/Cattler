using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI catIDUI;
    [SerializeField] private Image iconUI;
    [SerializeField] private TextMeshProUGUI descriptionUI;
    [SerializeField] private TextMeshProUGUI SkillUI;
    [SerializeField] private TextMeshProUGUI StatsUI;
    [SerializeField] private TextMeshProUGUI HP , ATK , ATKSPD, RNG, MVSPD;
    [SerializeField] private GameObject starGRP;
    [SerializeField] private GameObject star1, star2, star3; 
    [SerializeField] private TextMeshProUGUI LVLUI;
    [SerializeField] private TextMeshProUGUI HexCode;
    [SerializeField] private TextMeshProUGUI EXPUI;

    private void Awake()
    {
        AssignCardVisuals(); // correctly reference and asssign to correct card. then below will handle the insertion of text.
        Hide();
    }

    void AssignCardVisuals() //assign the card's of this script that is attached to.
    {

        // Auto-assign if not linked in Inspector
        if (nameUI == null)
            nameUI = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();

        if (iconUI == null)
            iconUI = transform.Find("ItemSprite")?.GetComponent<Image>();

        if (descriptionUI == null)
            descriptionUI = transform.Find("Description")?.GetComponent<TextMeshProUGUI>();

        if (SkillUI == null)
            SkillUI = transform.Find("Skill")?.GetComponent<TextMeshProUGUI>();

        if (StatsUI == null)
        {
            StatsUI = transform.Find("Stats")?.GetComponent<TextMeshProUGUI>();
            catIDUI = StatsUI.transform.Find("HexCode")?.GetComponent<TextMeshProUGUI>();
            HP = StatsUI.transform.Find("HP")?.GetComponent<TextMeshProUGUI>();
            ATK = StatsUI.transform.Find("ATK")?.GetComponent<TextMeshProUGUI>();
            ATKSPD = StatsUI.transform.Find("ATKSPD")?.GetComponent<TextMeshProUGUI>();
            RNG = StatsUI.transform.Find("RNG")?.GetComponent<TextMeshProUGUI>();
            MVSPD = StatsUI.transform.Find("MVSPD")?.GetComponent<TextMeshProUGUI>();
        }

        if(starGRP == null)
        {
            starGRP = transform.Find("RarityUI")?.gameObject;
            star1 = starGRP.transform.Find("Star")?.gameObject;
            star2 = starGRP.transform.Find("Star (1)")?.gameObject;
            star3 = starGRP.transform.Find("Star (2)")?.gameObject;
        }
        /*
        if (LVLUI == null)
            LVLUI = transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        if (EXPUI == null)
            EXPUI = transform.Find("Experience")?.GetComponent<TextMeshProUGUI>();
        */
    }

    /// <summary>
    /// Populates the UI with item data.
    /// </summary>
    public void Show(GameObject Item)
    {
        if(Item != null)
        {
            CatUnit catUnit = Item.GetComponent<CatUnit>();
            if (catUnit != null)
            {
                DisplayStars(catUnit.runtimeData.template);
                nameUI.text = catUnit.runtimeData.template.itemName;
                catIDUI.text = $"Cat ID: {catUnit.runtimeData.template.catID}";
                iconUI.sprite = catUnit.runtimeData.template.icon;
                descriptionUI.text = catUnit.runtimeData.template.description;
                SkillUI.text = catUnit.runtimeData.template.skillDesc;
                HP.text = $"{catUnit.runtimeData.maxHealth}";
                ATK.text = $"{catUnit.runtimeData.attackPower}";
                ATKSPD.text = $"{catUnit.runtimeData.attackSpeed}";
                RNG.text = $"{catUnit.runtimeData.attackRange}";
                MVSPD.text = $"{catUnit.runtimeData.movementSpeed}";
            }
            Item equipment = Item.GetComponent<Item>();
            if (equipment != null)
            {
                nameUI.text = equipment.runtimeData.template.itemName;
                iconUI.sprite = equipment.runtimeData.template.icon;
                descriptionUI.text = equipment.runtimeData.template.description;
                nameUI.text = equipment.runtimeData.template.itemName;
                SkillUI.text = equipment.runtimeData.template.AbilityDesc;
                HP.text = $"{equipment.runtimeData.health}";
                ATK.text = $"{equipment.runtimeData.attackPower}";
                ATKSPD.text = $"{equipment.runtimeData.attackSpeed}";
                RNG.text = $"{equipment.runtimeData.attackRange}";
                MVSPD.text = $"{equipment.runtimeData.movementSpeed}";
            } 
        }      
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the display panel.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowCatData(CatData catdata)
    {
        DisplayStars(catdata);
        nameUI.text = catdata.itemName;
        catIDUI.text = $"Cat ID: {catdata.catID}";
        iconUI.sprite = catdata.icon;
        descriptionUI.text = catdata.description;
        SkillUI.text = catdata.skillDesc;
        HP.text = $"{catdata.baseHealth}";
        ATK.text = $"{catdata.attackPower}";
        ATKSPD.text = $"{catdata.attackSpeed}";
        RNG.text = $"{catdata.attackRange}";
        MVSPD.text = $"{catdata.movementSpeed}";
    }

    void DisplayStars(CatData catData)
    {
        if(catData.Rarity == Rarity.Common)
        {
            star1.SetActive(true);
            star2.SetActive(false);
            star3.SetActive(false);
        }
        if(catData.Rarity == Rarity.Rare)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(false);
        }
        if(catData.Rarity == Rarity.Legendary)
        {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
        }
    }
}
