using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public bool isEnemy;
    public bool isCat;

    private EnemyUnit enemyUnit;
    private CatUnit catUnit;
    private HealthbarUIElement healthBarUIElement; //  Correct type!

    private GameObject healthbarLocation;
    private void OnEnable()
    {
        // Detect unit type
        enemyUnit = GetComponent<EnemyUnit>();
        catUnit = GetComponent<CatUnit>();
        healthbarLocation = transform.Find("HealthbarLocation")?.gameObject;

        isEnemy = enemyUnit != null;
        isCat = catUnit != null;

        //  Create UI element and store it properly
        healthBarUIElement = HealthbarUI.instance.CreateHealthbar(this);
    }

    private void Update()
    {
        if (healthBarUIElement == null) return;

        //  Update the position and value through HealthbarUIElement
        healthBarUIElement.UpdatePosition(healthbarLocation.transform.position);

        if (isEnemy && enemyUnit != null)
        {
            healthBarUIElement.UpdateHealth(enemyUnit.currentHealth, enemyUnit.maxHealth);
        }
        else if (isCat && catUnit != null)
        {
            healthBarUIElement.UpdateHealth(catUnit.runtimeData.currentHealth, catUnit.runtimeData.maxHealth);
        }
    }

    private void OnDisable()
    {
        if (healthBarUIElement != null)
        {
            healthBarUIElement.RemoveHealthbar();
            healthBarUIElement = null;
        }
    }
}
