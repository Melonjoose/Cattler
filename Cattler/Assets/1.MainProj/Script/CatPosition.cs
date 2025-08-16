using UnityEngine;

public class CatPosition : MonoBehaviour
{
    public GameObject positions1, positions2, positions3, positions4, positions5;
    public GameObject catPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.catPosition = this.gameObject;
        AutoAssignPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.catPosition.transform.position = positions1.transform.position;
            Debug.Log("button pressed for Position 1");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.catPosition.transform.position = positions2.transform.position;
            Debug.Log("button pressed for Position 2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.catPosition.transform.position = positions3.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.catPosition.transform.position = positions4.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            this.catPosition.transform.position = positions5.transform.position;
        }
    }

    public void AutoAssignPositions()
    {
        positions1 = GameObject.Find("Position1");
        positions2 = GameObject.Find("Position2");
        positions3 = GameObject.Find("Position3");
        positions4 = GameObject.Find("Position4");
        positions5 = GameObject.Find("Position5");
        if (positions1 == null || positions2 == null || positions3 == null || positions4 == null || positions5 == null)
        {
            Debug.LogError("One or more position GameObjects not found in the scene.");
        }
    }
}
