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
            StatsUI = transform.Find("Stats")?.GetComponent<TextMeshProUGUI>();

        if (LVLUI == null)
            LVLUI = transform.Find("Level")?.GetComponent<TextMeshProUGUI>();

        if (EXPUI == null)
            EXPUI = transform.Find("Experience")?.GetComponent<TextMeshProUGUI>();

        if (nameUI == null || iconUI == null || descriptionUI == null || SkillUI || StatsUI || LVLUI || EXPUI)
            Debug.LogWarning("DisplayItemUI: Missing one or more UI references!");

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
                StatsUI.text = $"HP : {catUnit.runtimeData.maxHealth} " +
                    $"\nATK : {catUnit.runtimeData.attackPower}" +
                    $"\nATK SPD : {catUnit.runtimeData.attackSpeed}" +
                    $"\nATK RNG : {catUnit.runtimeData.attackRange}" +
                    $"\nMV SPD : {catUnit.runtimeData.movementSpeed}";

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
