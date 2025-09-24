using Unity.VisualScripting;
using UnityEngine;

public class CatPositionManager : MonoBehaviour
{
    public static CatPositionManager instance;

    [Header("References")]
    public GameObject[] worldPositions;      // list of world pos (extracted from worldPositionGRP if needed)
    public GameObject[] catContainers;       // parent objects holding cats
    public DraggableIcon[] icons;            // icons linked to cats

    [Header("Indices")]
    public int[] iconIndices;                // indices of icons
    public int[] catIndices;                 // indices of cats
    public bool[] catInContainers;           // true if cat is in container

    [Header("Optional grouping")]
    public GameObject worldPositionGRP;      // Parent with all world pos

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AssignWorldPositions();
        InitializeArrays();
        UpdateManager();
    }

    void AssignWorldPositions()
    {
        if (worldPositionGRP != null)
        {
            int count = worldPositionGRP.transform.childCount;
            worldPositions = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                worldPositions[i] = worldPositionGRP.transform.GetChild(i).gameObject;
            }
        }
    }

    void InitializeArrays()
    {
        int length = Mathf.Max(icons.Length, catContainers.Length);
        iconIndices = new int[length];
        catIndices = new int[length];
        catInContainers = new bool[length];
    }

    public void UpdateManager()
    {
        UpdateIconIndices();
        UpdateCatIndices();
    }

    void UpdateIconIndices()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] != null)
                iconIndices[i] = icons[i].iconIndex;
        }
    }

    void UpdateCatIndices()
    {
        for (int i = 0; i < catContainers.Length; i++)
        {
            catIndices[i] = iconIndices[i];

            CatPosition catPos = catContainers[i].GetComponentInChildren<CatPosition>();
            if (catPos != null)
            {
                catPos.AssignCatIndex(catIndices[i]);
                catPos.AssignWorldPositions();
                catInContainers[i] = true;
            }
            else
            {
                catInContainers[i] = false;
            }
        }
    }

}
