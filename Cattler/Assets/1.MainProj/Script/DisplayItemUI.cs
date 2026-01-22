using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private Image iconUI;
    [SerializeField] private TextMeshProUGUI descriptionUI;
    [SerializeField] private TextMeshProUGUI SkillUI;
    [SerializeField] private TextMeshProUGUI StatsUI;
    [SerializeField] private TextMeshProUGUI HP , ATK , ATKSPD, RNG, MVSPD;
    [SerializeField] private TextMeshProUGUI LVLUI;
    [SerializeField] private TextMeshProUGUI EXPUI;

    private void Awake()
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
            HP = StatsUI.transform.Find("HP")?.GetComponent<TextMeshProUGUI>();
            ATK = StatsUI.transform.Find("ATK")?.GetComponent<TextMeshProUGUI>();
            ATKSPD = StatsUI.transform.Find("ATKSPD")?.GetComponent<TextMeshProUGUI>();
            RNG = StatsUI.transform.Find("RNG")?.GetComponent<TextMeshProUGUI>();
            MVSPD = StatsUI.transform.Find("MVSPD")?.GetComponent<TextMeshProUGUI>();
        }

        if (LVLUI == null)
            LVLUI = transform.Find("Level")?.GetComponent<TextMeshProUGUI>();

        if (EXPUI == null)
            EXPUI = transform.Find("Experience")?.GetComponent<TextMeshProUGUI>();

        Hide();
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
                nameUI.text = catUnit.runtimeData.template.itemName;
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
}
