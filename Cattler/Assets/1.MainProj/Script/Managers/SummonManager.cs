using System;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SummonManager;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance;
    public int summonCost = 100;
    public Animator summonAnimator;

    public CatData[] commonCats;
    public CatData[] rareCats;
    public CatData[] legendaryCats;


    public Canvas summonCanvas;
    public GameObject tapToRevealPage;

    public DisplayItemUI commonUI,rareUI,legendaryUI; //the card itself.
    private DisplayItemUI chosenUI;
    public GameObject summonCatDisplay; // holds common,rare,legendary
    public GameObject closeSummonPage;
    public CatData currentRolledCat;
    public Rarity currentRolledRarity;

    private void Awake()
    {

        // Load all CatData assets from subfolders
        commonCats = Resources.LoadAll<CatData>("Cats/Common");
        rareCats = Resources.LoadAll<CatData>("Cats/Rare");
        legendaryCats = Resources.LoadAll<CatData>("Cats/Legendary");

        summonCanvas.transform.localPosition = new Vector3(0, -10, 0);
        instance = this;
        tapToRevealPage.SetActive(false);
        InitializeCards();// summoncat holding all the common,rare,legendary.
        closeSummonPage.SetActive(false);
}

    void InitializeCards()
    {
        commonUI.gameObject.SetActive(false);
        rareUI.gameObject.SetActive(false);
        legendaryUI.gameObject.SetActive(false);
        summonCatDisplay.SetActive(false);
    }


    public enum Rarity { Common, Rare, Legendary }

    private Rarity RollRarity(bool isPremium) // FIRST roll to see what tier you get.
    {
        float roll = UnityEngine.Random.Range(0f, 1f);
        Rarity result;

        if (!isPremium)
        {
            if (roll < 0.80f) result = Rarity.Common;
            else if (roll < 0.95f) result = Rarity.Rare;
            else result = Rarity.Legendary;
        }
        else
        {
            if (roll < 0.50f) result = Rarity.Common;
            else if (roll < 0.80f) result = Rarity.Rare;
            else result = Rarity.Legendary;
        }

        Debug.Log($"Rolled {roll:F2}, resulting in rarity: {result}");
        return result;
    }

    private CatData RollCat(Rarity rarity)  //SECOND ROLL to see what cat you get.
    {
        CatData[] pool = null;

        switch (rarity)
        {
            case Rarity.Common: pool = commonCats; break;
            case Rarity.Rare: pool = rareCats; break;
            case Rarity.Legendary: pool = legendaryCats; break;
        }

        if (pool == null || pool.Length == 0)
        {
            Debug.LogError($"No cats found for rarity {rarity}!");
            return null;
        }

        // Even distribution by default
        int index = UnityEngine.Random.Range(0, pool.Length);
        return pool[index];
    }

    public void Summon(bool isPremium = false)
    {
        Rarity rarity = RollRarity(isPremium);
        currentRolledCat = RollCat(rarity);
        currentRolledRarity = rarity;

        if (currentRolledCat == null) return;

        Inventory.instance.InstantiateNewCat(currentRolledCat);
        Debug.Log($"Summoned {rarity} cat: {currentRolledCat.itemName}!");
    }


    void RarityChecker(Rarity rarity)
    {
        if (rarity == Rarity.Common)
        {
            chosenUI = commonUI;
            commonUI.gameObject.SetActive(true);
            rareUI.gameObject.SetActive(false);
            legendaryUI.gameObject.SetActive(false);
        }
        else if (rarity == Rarity.Rare)
        {
            chosenUI = rareUI;
            commonUI.gameObject.SetActive(false);
            rareUI.gameObject.SetActive(true);
            legendaryUI.gameObject.SetActive(false);
        }
        else if (rarity == Rarity.Legendary)
        {
            chosenUI = legendaryUI;
            commonUI.gameObject.SetActive(false);
            rareUI.gameObject.SetActive(false);
            legendaryUI.gameObject.SetActive(true);
        }
        
    }
    void DisplayGachaResult()
    {
        summonCatDisplay.SetActive(true);
        RarityChecker(currentRolledRarity); // use rolled cat’s rarity
        chosenUI.ShowCatData(currentRolledCat);
        summonAnimator.SetTrigger("Summon");
    }


    public void SummonButtonPressed() //to add to button onclick event
    {
        if (Inventory.instance.inventoryList.Count >= Inventory.instance.currentCapacity)
        {
            CommentaryManager.instance.AddDialogueToQueue(4); // team is full
            Debug.Log("Inventory is full! Unable to summon Cats");
            return;
        }

        if (Currency.instance.ink < summonCost)
        {
            CommentaryManager.instance.AddDialogueToQueue(8); // Not enough currency. We should go and clear out some inklings.
            Debug.Log("Not enough ink to summon!");
            return;
        }
        Currency.instance.AddInk(-summonCost); // Deduct summon cost
        Summon();

        summonCatDisplay.SetActive(true);   // activate parent first
        RarityChecker(currentRolledRarity);              // then toggle children
        commonUI.gameObject.SetActive(false);
        rareUI.gameObject.SetActive(false);
        legendaryUI.gameObject.SetActive(false);
        OpenTapToRevealPage();
    }

    void OpenTapToRevealPage()
    {        
        tapToRevealPage.SetActive(true);
    }

    public void TapToReveal()
    {
        DisplayGachaResult();
        closeSummonPage.SetActive(true);
    }

    public void CloseSummonPages()
    {
        if(tapToRevealPage.activeSelf == false && summonCatDisplay.activeSelf == false && closeSummonPage.activeSelf)
        {
            return; //both pages are already closed
        }
        tapToRevealPage.SetActive(false);
        commonUI.gameObject.SetActive(false);
        rareUI.gameObject.SetActive(false);
        legendaryUI.gameObject.SetActive(false);
        summonCatDisplay.SetActive(false);
        closeSummonPage.SetActive(false);
    }
}
