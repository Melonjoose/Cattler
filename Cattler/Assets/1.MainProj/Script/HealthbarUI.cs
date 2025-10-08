using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    public static HealthbarUI instance;
    public GameObject healthBarPrefab;

    private List<Slider> activeBars = new List<Slider>();
    private Camera mainCam;
    public GameObject healthBarStorage; // Assign in Inspector (your main UI canvas)

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;

    }

    // Create and attach a healthbar for a unit
    public HealthbarUIElement CreateHealthbar(Healthbar target)
    {
        Debug.Log("Creating healthbar for " + target.name);

        // Spawn the bar under the UI canvas
        GameObject newBar = Instantiate(healthBarPrefab, healthBarStorage.transform);

        Slider slider = newBar.GetComponent<Slider>();
        var element = newBar.AddComponent<HealthbarUIElement>();

        element.Initialize(target, slider, mainCam);
        activeBars.Add(slider);
        return element;
    }

    public void RemoveHealthbar(Slider slider)
    {
        if (slider != null)
        {
            activeBars.Remove(slider);
            Destroy(slider.gameObject);
        }
    }
}
