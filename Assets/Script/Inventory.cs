using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Inventory : MonoBehaviour
{
    public int rows = 4;
    public int columns = 4;
    public float slotSize = 1.0f;
    public GameObject slotPrefab;
    public Vector3 location;
    private GameObject[,] slots;

    void Start()
    {
        CreateGrid();
        SetPositionToTarget();
    }

    void CreateGrid()
    {
        slots = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * slotSize, i * slotSize, 0);
                GameObject slot = Instantiate(slotPrefab, position, Quaternion.identity, transform);
                slots[i, j] = slot;
            }
        }
    }

    public Vector3 GetNearestSlotPosition(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Vector3 nearestSlotPosition = Vector3.zero;

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(position, slot.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSlotPosition = slot.transform.position;
            }
        }

        return nearestSlotPosition;
    }

    public void SetPositionToTarget()
    {
        if (location != null)
        {
            transform.position = location;
        }
    }
}
