using TMPro;
using Unity.VisualScripting;
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

    public int hpChange;
    public int atkChange;
    public float atkspdChange;
    public float atkrngChange;
    public float mvspdChange;

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
        textBoxGRP = transform.Find("TextBox")?.gameObject;
        StatGRP = textBoxGRP.transform.Find("Stat")?.gameObject;

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

            // Use FormatStat for all stats
            CalculateItemStatsChanges(); // returns your StatChanges struct

            HP_StatsUI.text = FormatStat("HP", catUnit.runtimeData.maxHealth, hpChange, 0);
            ATK_StatsUI.text = FormatStat("ATK", catUnit.runtimeData.attackPower, atkChange, 0);
            ATKSPD_StatsUI.text = FormatStat("ATK SPD", catUnit.runtimeData.attackSpeed, atkspdChange, 2);
            ATKRNG_StatsUI.text = FormatStat("ATK RNG", catUnit.runtimeData.attackRange, atkrngChange, 2);
            MVSPD_StatsUI.text = FormatStat("MV SPD", catUnit.runtimeData.movementSpeed, mvspdChange, 2);
        }
        else
        {
            RemoveCatStats();
        }
    }

    public void RemoveCatStats() // default state.
    {
        nameUI.text = "Add a cat above to preview!";
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


    public void CalculateItemStatsChanges()
    {
        Item Hat = PreviewManager.instance.hat;
        Item WeaponL = PreviewManager.instance.weaponL;
        Item WeaponR = PreviewManager.instance.weaponR;

        hpChange = (Hat?.runtimeData?.health ?? 0) +
                       (WeaponL?.runtimeData?.health ?? 0) +
                       (WeaponR?.runtimeData?.health ?? 0);

        atkChange = (Hat?.runtimeData?.attackPower ?? 0) +
                       (WeaponL?.runtimeData?.attackPower ?? 0) +
                       (WeaponR?.runtimeData?.attackPower ?? 0);

        atkspdChange = (Hat?.runtimeData?.attackSpeed ?? 0f) +
                       (WeaponL?.runtimeData?.attackSpeed ?? 0f) +
                       (WeaponR?.runtimeData?.attackSpeed ?? 0f);

        atkrngChange = (Hat?.runtimeData?.attackRange ?? 0f) +
                       (WeaponL?.runtimeData?.attackRange ?? 0f) +
                       (WeaponR?.runtimeData?.attackRange ?? 0f);

        mvspdChange = (Hat?.runtimeData?.movementSpeed ?? 0f) +
                       (WeaponL?.runtimeData?.movementSpeed ?? 0f) +
                       (WeaponR?.runtimeData?.movementSpeed ?? 0f);
    }



    public void UpdateStats()
    {
        CatUnit catUnit = selectedCat.GetComponent<CatUnit>();
        if (catUnit != null)
        {
            nameUI.text = catUnit.runtimeData.template.itemName;
            descriptionUI.text = catUnit.runtimeData.template.description;
            SkillUI.text = catUnit.runtimeData.template.skillDesc;

            // Use FormatStat for all stats
            CalculateItemStatsChanges(); // returns your StatChanges struct

            HP_StatsUI.text = FormatStat("HP", catUnit.runtimeData.maxHealth, hpChange, 0);
            ATK_StatsUI.text = FormatStat("ATK", catUnit.runtimeData.attackPower, atkChange, 0);
            ATKSPD_StatsUI.text = FormatStat("ATK SPD", catUnit.runtimeData.attackSpeed, atkspdChange, 2);
            ATKRNG_StatsUI.text = FormatStat("ATK RNG", catUnit.runtimeData.attackRange, atkrngChange, 2);
            MVSPD_StatsUI.text = FormatStat("MV SPD", catUnit.runtimeData.movementSpeed, mvspdChange, 2);
        }
    }

    private string FormatStat(string label, float baseValue, float changeValue, int decimals = 2)
    {
        float finalValue = baseValue + changeValue;
        string changeText = "";

        if (changeValue > 0)
            changeText = $" <color=green>(+{changeValue.ToString($"F{decimals}")})</color>";
        else if (changeValue < 0)
            changeText = $" <color=red>({changeValue.ToString($"F{decimals}")})</color>";

        return $"{label} : {baseValue.ToString($"F{decimals}")}{changeText}";
    }



}
