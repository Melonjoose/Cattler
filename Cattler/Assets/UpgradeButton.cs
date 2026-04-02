using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Data
    public Upgrade upgrade; // link to upgrade info

    //UI Displays
    public Image icon; 
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;

    public Button button;
    private Vector3 defaultScale;
    private Vector3 hoverScale;

    void Start()
    {
        InitializeButton();
    }

    public void InitializeButton()
    {
        button = GetComponent<Button>();
        defaultScale = transform.localScale;
        hoverScale = defaultScale * 1.2f;
        //reference all the ui components.
        // > drag inspector.


        // Add click listener
        button.onClick.AddListener(OnClickUpgrade);

        isHovered = false;
    }

    void Update()
    {
        // Smooth scaling effect
        if (isHovered)
            transform.localScale = Vector3.Lerp(transform.localScale, hoverScale, Time.deltaTime * 10f);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * 10f);
    }

    public void UpdateUpgradeButtonUI() //update this buttons displays
    {
        if (icon != null) icon.sprite = upgrade.iconSprite;
        if (upgradeText != null) upgradeText.text = upgrade.upgradeName;
        if (levelText != null) levelText.text = $"LV. {upgrade.currentLevel} >> LV. {upgrade.currentLevel + 1}";
        if (costText != null) costText.text = $"{upgrade.price}";
    }

    public bool isHovered { get; private set; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    private void OnClickUpgrade() //when this is clicked at menu.. open page.
    {
        Debug.Log($"{upgrade.name} button is pressed");
        // Reset scale on click
        transform.localScale = defaultScale;
        UpgradeManager.instance.descBox.upgrade = upgrade;
        UpgradeManager.instance.descBox.EnableThis();
        UpgradeManager.instance.descBox.UpdateDescriptionBox(upgrade);
    }

}