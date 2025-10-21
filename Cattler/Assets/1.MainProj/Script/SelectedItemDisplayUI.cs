using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemDisplayUI : MonoBehaviour
{
    public static SelectedItemDisplayUI instance;
    [Header("UI References")]
    [SerializeField] private GameObject textBoxGRP;
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI descriptionUI;
    [SerializeField] private TextMeshProUGUI SkillUI;
    [SerializeField] private TextMeshProUGUI StatsUI;
    [SerializeField] private TextMeshProUGUI LVLUI;
    [SerializeField] private TextMeshProUGUI EXPUI;

    //[SerializeField] private GameObject catSlot;
    [SerializeField] private SnappableLocation catSlot;

    private void Awake()
    {
        instance = this;

        InitializeReferences();

        NothingInSelectedItemDisplayUI(null);
    }
    private void OnEnable()
    {
        catSlot.OnItemPlaced += ShowCatStats;
        catSlot.OnItemRemoved += NothingInSelectedItemDisplayUI;
    }
    private void OnDisable()
    {
        catSlot.OnItemPlaced -= ShowCatStats;
        catSlot.OnItemRemoved -= NothingInSelectedItemDisplayUI;
    }


    void InitializeReferences()
    {
        if (textBoxGRP == null)
            textBoxGRP = transform.Find("TextBox")?.GetComponent<GameObject>();

        // Auto-assign if not linked in Inspector
        if (nameUI == null)
            nameUI = textBoxGRP.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();

        if (descriptionUI == null)
            descriptionUI = textBoxGRP.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();

        if (SkillUI == null)
            SkillUI = textBoxGRP.transform.Find("Skill")?.GetComponent<TextMeshProUGUI>();

        if (StatsUI == null)
            StatsUI = textBoxGRP.transform.Find("Stats")?.GetComponent<TextMeshProUGUI>();

        if (LVLUI == null)
            LVLUI = textBoxGRP.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();

        if (EXPUI == null)
            EXPUI = textBoxGRP.transform.Find("Experience")?.GetComponent<TextMeshProUGUI>();

        if (catSlot == null)
            catSlot = transform.Find("CatImage")?.GetComponent<SnappableLocation>();

        if (nameUI == null || descriptionUI == null || SkillUI == null || StatsUI == null || LVLUI == null || EXPUI == null || catSlot == null)
            Debug.Log("DisplayItemUI: Missing one or more UI references!");
    } 

    public void ShowCatStats(SnappableLocation catSlot)
    {
        if(catSlot != null)
        {
            if (catSlot.currentItem != null)
            {
               InventoryIcon cat = catSlot.currentItem;
                CatUnit catUnit = cat.GetComponent<CatUnit>();
                if (catUnit != null)
                {
                    nameUI.text = catUnit.runtimeData.template.itemName;
                    descriptionUI.text = catUnit.runtimeData.template.description;
                    SkillUI.text = catUnit.runtimeData.template.skillDesc;
                    StatsUI.text = $"HP : {catUnit.runtimeData.maxHealth} " +
                        $"\nATK : {catUnit.runtimeData.attackPower}" +
                        $"\nATK SPD : {catUnit.runtimeData.attackSpeed}" +
                        $"\nATK RNG : {catUnit.runtimeData.attackRange}" +
                        $"\nMV SPD : {catUnit.runtimeData.movementSpeed}";
                }
            }
        }
    }

    public void NothingInSelectedItemDisplayUI(SnappableLocation catSlot) // default state.
    {
        nameUI.text = "No cats to preview!";
        descriptionUI.text = string.Empty;
        descriptionUI.text = string.Empty;
        SkillUI.text = string.Empty;
        StatsUI.text = string.Empty;
        LVLUI.text = string.Empty;
        EXPUI.text = string.Empty;
        //null everything
    }
}
