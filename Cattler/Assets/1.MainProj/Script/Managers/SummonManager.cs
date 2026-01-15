using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance;
    public int summonCost = 100;
    public Animator summonAnimator;
    public GameObject tapToRevealPage;
    public GameObject displaySummonedCatPage;
    public GameObject closeSummonPage;
    public CatData currentRolledCat;

    public TextMeshProUGUI catNameText;
    public RawImage catSummonedImage;
    public TextMeshProUGUI descText;



    [System.Serializable]
    public class GachaPoolEntry
    {
        public CatData catData;
        public float weight; // probability weight
    }

    public GachaPoolEntry[] gachaPool; // assign in inspector
    public event Action onGacha;



    private void OnEnable()
    {
        onGacha += GachaSequence;
    }

    private void Awake()
    {
        instance = this;
        tapToRevealPage.SetActive(false);
        displaySummonedCatPage.SetActive(false);
        closeSummonPage.SetActive(false);
    }

    public CatData Roll()
    {
        float totalWeight = 0f;
        foreach (var entry in gachaPool)
            totalWeight += entry.weight;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in gachaPool)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.catData;
        }

        return null; // should never happen
    }

    public void Summon()
    {
        currentRolledCat = Roll();
        Debug.Log("Rolled cat = " + currentRolledCat);

        if (currentRolledCat == null)
        {
            Debug.LogError("No cat was rolled!");
            return;
        }

        Inventory.instance.InstantiateNewCat(currentRolledCat);
        Debug.Log($"Summoned {currentRolledCat.itemName}!");
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
