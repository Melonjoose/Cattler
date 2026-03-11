using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject displaySummonedCatPage;
    public GameObject closeSummonPage;
    public CatData currentRolledCat;

    public TextMeshProUGUI catNameText;
    public RawImage catSummonedImage;
    public TextMeshProUGUI descText;

    public event Action onGacha;

    private void OnEnable()
    {
        onGacha += GachaSequence;
    }

    private void Awake()
    {

        // Load all CatData assets from subfolders
        commonCats = Resources.LoadAll<CatData>("Cats/Common");
        rareCats = Resources.LoadAll<CatData>("Cats/Rare");
        legendaryCats = Resources.LoadAll<CatData>("Cats/Legendary");

        summonCanvas.transform.localPosition = new Vector3(0, -10, 0);
        instance = this;
        tapToRevealPage.SetActive(false);
        displaySummonedCatPage.SetActive(false);
        closeSummonPage.SetActive(false);
}

public enum Rarity { Common, Rare, Legendary }

    private Rarity RollRarity(bool isPremium)
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


    private CatData RollCat(Rarity rarity)
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

        if (currentRolledCat == null) //safety
        {
            Debug.LogError("No cat was rolled!");
            return;
        }

        Inventory.instance.InstantiateNewCat(currentRolledCat);
        Debug.Log($"Summoned {rarity} cat: {currentRolledCat.itemName}!");
        onGacha?.Invoke();
    }



    void GachaSequence()
    {
        Debug.Log("Gacha sequence playing...");
    }

    void UpdateCatDisplay(CatData catData)
    {
        catNameText.text = catData.itemName;
        catSummonedImage.texture = catData.icon.texture;
        descText.text = catData.description;
    }

    void DisplayGachaResult()
    {
        displaySummonedCatPage.SetActive(true);
        UpdateCatDisplay(currentRolledCat);
        summonAnimator.SetTrigger("Summon");

        Debug.Log("Displaying gacha result to player...");
    }

    public void SummonButtonPressed() //to add to button onclick event
    {
        if (TeamManager.instance.currentTeamSize >= TeamManager.instance.availableTeamSlots)
        {
            Debug.Log("No free team slots available!");
            return;
        }

        if (Currency.instance.ink < summonCost)
        {
            Debug.Log("Not enough ink to summon!");
            return;
        }
        Currency.instance.AddInk(-summonCost); // Deduct summon cost
        Summon();
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
        if(tapToRevealPage.activeSelf == false && displaySummonedCatPage.activeSelf == false && closeSummonPage.activeSelf)
        {
            return; //both pages are already closed
        }
        tapToRevealPage.SetActive(false);
        displaySummonedCatPage.SetActive(false);
        closeSummonPage.SetActive(false);
    }
}
