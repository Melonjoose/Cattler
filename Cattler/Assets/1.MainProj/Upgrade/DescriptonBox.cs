using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionBox : MonoBehaviour
{
    public Upgrade upgrade; //to let this descriptionbox link to the upgrade to get it's info.

    public CanvasGroup canvasGRP;

    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI upgradeDescText;
    public TextMeshProUGUI currentUpgradeText;
    public TextMeshProUGUI nextUpgradeText;
    public TextMeshProUGUI priceText;

    public Button purchaseButton; //use to buy.
    public Button closeButton; //use to buy.


    private void Start()
    {
        closeButton.onClick.AddListener(ClosePage);
        purchaseButton.onClick.AddListener(OnBuyClick);
        canvasGRP = GetComponent<CanvasGroup>();
        this.transform.position = UpgradeManager.instance.upgradeUI.descriptionBoxDefaultPos.position;
        DisableThis();
    }

    public void DisableThis()
    {
        canvasGRP.alpha = 0; //hide the description box at the start of the game.
        canvasGRP.interactable = false; //hide the description box at the start of the game.
        canvasGRP.blocksRaycasts = false;
    }
    public void EnableThis()
    {
        canvasGRP.alpha = 1; //hide the description box at the start of the game.
        canvasGRP.interactable = true; //hide the description box at the start of the game.
        canvasGRP.blocksRaycasts = true;
    }

    public void UpdateDescriptionBox(Upgrade upgradeButton)
    {
        if(upgrade == null) { return; }
        
        upgrade = upgradeButton;
        upgradeNameText.text = $"{upgrade.upgradeName} - Lv {upgrade.currentLevel}";
        upgradeDescText.text = upgrade.description;
        currentUpgradeText.text = $"+{upgrade.currentTotalValueAdded} {upgrade.summaryUpgradeText}";
        nextUpgradeText.text = $"+{upgrade.nextTotalValueAdded} {upgrade.summaryUpgradeText}";
        priceText.text = $"{ upgrade.price}";
    }

    public void OnBuyClick() //when buy button is clicked.
    {
        if(upgrade.currentLevel >= upgrade.maxlevel)
        {
            Debug.Log("This upgrade has reached is maximum level.");
            nextUpgradeText.text = "This upgrade has been maxed out";
            AudioManager.instance.PlaySFX("SoftDeny");
            return;
        }
        if (Currency.instance.ink >= upgrade.price) //have enough money 
        {
            //sound and effects
            Debug.Log("purchase accepted");
            UpgradeManager.instance.PurchaseUpgrade(upgrade.price);
            //update the UpgradeButton located at the upgrademenu.
            UpgradeManager.instance.upgradeUI.UpdateAllUpgradeButtonsUI();
            AudioManager.instance.PlaySFX("Purchase");

        }
        else if (Currency.instance.ink <= upgrade.price) //not enough money.
        {
            Debug.Log("Player does not have enough ink");
            AudioManager.instance.PlaySFX("SoftDeny");
            CommentaryManager.instance.AddDialogueToQueue(08);

        }
        else if (upgrade.currentLevel >= upgrade.maxlevel)
        {
            Debug.Log("levels are maxed out and cannot be upgraded");
            AudioManager.instance.PlaySFX("SoftDeny");
            CommentaryManager.instance.AddDialogueToQueue(09);
        }

    }

    public void ClosePage()
    {
        DisableThis();
        UpdateDescriptionBox(upgrade);
    }
}
