using UnityEngine;
using UnityEngine.UI;

public class CatIconUI : MonoBehaviour
{
    public static CatIconUI instance;

    [System.Serializable]
    public class CatIconSlot
    {
        public Image icon;
        public Slider healthBar;
    }

    [Header("UI Slots")]
    public CatIconSlot[] uiSlots; // assign 5 in Inspector

    [Header("Containers")]
    public GameObject[] catContainer; // assign containers in Inspector

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        RefreshIcons();
    }

    private void Update()
    {
        RefreshIcons(); // left here for simplicity, optimize later if needed
    }

    private void RefreshIcons()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i >= catContainer.Length) return; // safety check

            // If a cat exists in container slot
            if (catContainer[i].transform.childCount > 0)
            {
                CatUnit cat = catContainer[i].transform.GetChild(0).GetComponent<CatUnit>();
                if (cat != null)
                {
                    uiSlots[i].icon.sprite = cat.runtimeData.template.icon; 
                    uiSlots[i].icon.gameObject.SetActive(true);

                    uiSlots[i].healthBar.gameObject.SetActive(true);
                    uiSlots[i].healthBar.maxValue = cat.runtimeData.maxHealth;
                    uiSlots[i].healthBar.value = cat.runtimeData.currentHealth;
                }
            }
            else
            {
                // No cat in this container → hide icon & healthbar
                uiSlots[i].icon.gameObject.SetActive(false);
                uiSlots[i].healthBar.gameObject.SetActive(false);
            }
        }
    }

}
