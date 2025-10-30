using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemDisplayUI : MonoBehaviour
{
    public static SelectedItemDisplayUI instance;
    [Header("UI References")]
    [SerializeField] private GameObject textBoxGRP;
    [SerializeField] private GameObject StatGRP;
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI descriptionUI;
    [SerializeField] private TextMeshProUGUI SkillUI;
    [SerializeField] private TextMeshProUGUI HP_StatsUI;
    [SerializeField] private TextMeshProUGUI ATK_StatsUI;
    [SerializeField] private TextMeshProUGUI ATKSPD_StatsUI;
    [SerializeField] private TextMeshProUGUI ATKRNG_StatsUI;
    [SerializeField] private TextMeshProUGUI MVSPD_StatsUI;
    [SerializeField] private TextMeshProUGUI LVLUI;
    [SerializeField] private TextMeshProUGUI EXPUI;

    //Items that have been added into the Displayer.
    [SerializeField] private InventoryIcon selectedCat;
    [SerializeField] private InventoryIcon selectedHat;
    [SerializeField] private InventoryIcon selectedWeapon1;
    [SerializeField] private InventoryIcon selectedWeapon2;

    private void Awake()
    {
        instance = this;

        InitializeReferences();

        RemoveCatStats(); //default State
    }
    private void OnEnable()
    {
        //catSlot.OnItemPlaced += ShowCatStats;
        //catSlot.OnItemRemoved += ShowCatStats;
    }
    private void OnDisable()
    {
        //catSlot.OnItemPlaced -= ShowCatStats;
        //catSlot.OnItemRemoved -= ShowCatStats;
    }


    void InitializeReferences()
    {
        if (textBoxGRP == null)
            textBoxGRP = transform.Find("TextBox")?.GetComponent<GameObject>();

        if (StatGRP == null)
            StatGRP = transform.Find("Stat")?.GetComponent<GameObject>();

        // Auto-assign if not linked in Inspector
        if (nameUI == null)
            nameUI = textBoxGRP.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();

        if (descriptionUI == null)
            descriptionUI = textBoxGRP.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();

        if (SkillUI == null)
            SkillUI = textBoxGRP.transform.Find("Skill")?.GetComponent<TextMeshProUGUI>();

        //------------------------------------Stats--------------------------------//
        if (HP_StatsUI == null)
            HP_StatsUI = StatGRP.transform.Find("HP")?.GetComponent<TextMeshProUGUI>();

        if (ATK_StatsUI == null)
            ATK_StatsUI = StatGRP.transform.Find("ATK")?.GetComponent<TextMeshProUGUI>();

        if (ATKSPD_StatsUI == null)
            ATKSPD_StatsUI = StatGRP.transform.Find("ATKSPD")?.GetComponent<TextMeshProUGUI>();

        if (ATKRNG_StatsUI == null)
            ATKRNG_StatsUI = StatGRP.transform.Find("ATKRNG")?.GetComponent<TextMeshProUGUI>();

        if (MVSPD_StatsUI == null)
            MVSPD_StatsUI = StatGRP.transform.Find("MVSPD")?.GetComponent<TextMeshProUGUI>();
        //------------------------------------Stats--------------------------------//

        if (LVLUI == null)
            LVLUI = textBoxGRP.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();

        if (EXPUI == null)
            EXPUI = textBoxGRP.transform.Find("Experience")?.GetComponent<TextMeshProUGUI>();


    } 

    public void ShowCatStats(InventoryIcon catIcon)
    {
        selectedCat = catIcon;
            CatUnit catUnit = selectedCat.GetComponent<CatUnit>();
        if (catUnit != null)
        {
            nameUI.text = catUnit.runtimeData.template.itemName;
            descriptionUI.text = catUnit.runtimeData.template.description;
            SkillUI.text = catUnit.runtimeData.template.skillDesc;
            HP_StatsUI.text = $"HP : {catUnit.runtimeData.maxHealth} ";
            ATK_StatsUI.text = $"ATK : {catUnit.runtimeData.attackPower} ";
            ATKSPD_StatsUI.text = $"ATK SPD : {catUnit.runtimeData.attackSpeed} ";
            ATKRNG_StatsUI.text = $"ATK RNG : {catUnit.runtimeData.attackRange} ";
            MVSPD_StatsUI.text = $"MV SPD : {catUnit.runtimeData.movementSpeed} ";
        }
        else
        {
            RemoveCatStats();
        }
        
    }

    public void RemoveCatStats() // default state.
    {
        nameUI.text = "No cats to preview!";
        descriptionUI.text = string.Empty;
        descriptionUI.text = string.Empty;
        SkillUI.text = string.Empty;

        HP_StatsUI.text = string.Empty;
        ATK_StatsUI.text = string.Empty;
        ATKSPD_StatsUI.text = string.Empty;
        ATKRNG_StatsUI.text = string.Empty;
        MVSPD_StatsUI.text = string.Empty;

        LVLUI.text = string.Empty;
        EXPUI.text = string.Empty;
        selectedCat = null;
    }

    //replacing cat does 1.show then 2.remove??? or the other way round
}
