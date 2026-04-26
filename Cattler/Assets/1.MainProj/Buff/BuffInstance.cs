using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class BuffInstance
{
    public Unit unit; // The unit this debuff is applied to
    public CatUnit catUnit; // The cat is where you access the runtimedata.
    public Buff buff;
    public Buff.BuffType buffType;
    public Buff.ValueIncreaseType valueIncreaseType;

    public Sprite sprite; // For UI display, if needed
    public GameObject buffIcon;

    public float remainingTime;
    public float valueIncreaseOfthisBuff;

    public BuffInstance(Buff buff, Unit unit, float duration)
    {
        this.buff = buff;
        this.buffType = buff.buffType;
        this.valueIncreaseType = buff.valueIncreaseType;
        this.unit = unit;
        catUnit = unit as CatUnit;
        this.sprite = buff.sprite;
        this.remainingTime = duration;
    }
    public bool Tick(float deltaTime)
    {
        remainingTime -= deltaTime;
        return remainingTime <= 0;
    }

    public void ShowUI()
    {
        // Implement UI display logic here, e.g., show debuff icon above unit
        // show UI above enemy head. when the debuff is at the last 1 second, the icon will start flashing. when the debuff is removed, the icon will disappear.
        if (unit != null && sprite != null)
        {
            buffIcon = GameObject.Instantiate(BuffManager.instance.iconPrefab, unit.transform.position + Vector3.up * 2, Quaternion.identity);
            //set debufficon as a child of this unit's.debuffcanvas
            buffIcon.transform.SetParent(unit.GetComponent<Unit>().debuffCanvas.transform, false);
            buffIcon.name = $"{buff.buffName} Icon"; // Name the icon for easier debugging
            Image image = buffIcon.GetComponent<Image>();

            if (image != null)
            {
                image.sprite = sprite;
            }
        }
    }

    public void BlinkingUI()
    {
        if (buffIcon == null) return;

        Image image = buffIcon.GetComponent<Image>();
        if (image == null) return;

        // Blink speed (how many times per second)
        float blinkSpeed = 6f;

        // Ping-pong between 0 and 1 based on time
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void RemoveUI()
    {
        if (buffIcon != null)
        {
            GameObject.Destroy(buffIcon);
            buffIcon = null;
        }
    }

    public void ApplyBuffEffect() //apply this debuffeffect
    {
        IncreaseStat(buff.value);
    }

    public void RemoveBuffEffect()
    {
        RemoveIncreaseStat(valueIncreaseOfthisBuff);
    }


    // -------------- Buffs effect logic below ---------------//

    public void IncreaseStat(float stat)
    {
        if (buff != null)
        {
            if (buffType == Buff.BuffType.AttackSpeed)
            {
                if (valueIncreaseType == Buff.ValueIncreaseType.Flat)
                {
                    catUnit.runtimeData.attackSpeed += buff.value;
                    valueIncreaseOfthisBuff = buff.value;
                }
                else if (valueIncreaseType == Buff.ValueIncreaseType.Percentage)
                {
                    float increase = catUnit.runtimeData.attackSpeed * (buff.value / 100f);
                    catUnit.runtimeData.attackSpeed += increase;
                    valueIncreaseOfthisBuff = increase;
                }
            }

            if (buffType == Buff.BuffType.AttackPower)
            {
                if (valueIncreaseType == Buff.ValueIncreaseType.Flat)
                {
                    catUnit.runtimeData.attackPower += (int)buff.value;
                    valueIncreaseOfthisBuff = buff.value;
                }
                else if (valueIncreaseType == Buff.ValueIncreaseType.Percentage)
                {
                    int increase = Mathf.RoundToInt(catUnit.runtimeData.attackPower * (buff.value / 100f));
                    catUnit.runtimeData.attackPower += increase;
                    valueIncreaseOfthisBuff = increase;
                }
            }

            if (buffType == Buff.BuffType.Range)
            {
                if (valueIncreaseType == Buff.ValueIncreaseType.Flat)
                {
                    catUnit.runtimeData.attackRange += buff.value;
                    valueIncreaseOfthisBuff = buff.value;
                }
                else if (valueIncreaseType == Buff.ValueIncreaseType.Percentage)
                {
                    float increase = catUnit.runtimeData.attackRange * (buff.value / 100f);
                    catUnit.runtimeData.attackRange += increase;
                    valueIncreaseOfthisBuff = increase;
                }
            }
        }
    }

    public void RemoveIncreaseStat(float stat)
    {
        if (buff != null)
        {
            if (buffType == Buff.BuffType.AttackSpeed)
            {
                catUnit.runtimeData.attackSpeed -= stat;
            }

            if (buffType == Buff.BuffType.AttackPower)
            {
                catUnit.runtimeData.attackPower -= (int)stat;
            }

            if (buffType == Buff.BuffType.Range)
            {
                catUnit.runtimeData.attackRange -= stat; // keep consistent with float
            }
        }
    }


    private float regenTimer = 0f;
    public void HealthRegen(float deltaTime)
    {
        if (buff != null && buffType == Buff.BuffType.HealthRegen)
        {
            regenTimer += deltaTime;

            // Heal once per second
            if (regenTimer >= 1f)
            {
                regenTimer = 0f;

                // Apply healing
                catUnit.runtimeData.currentHealth += (int)buff.value;

                // Clamp to max health
                if (catUnit.runtimeData.currentHealth > catUnit.runtimeData.maxHealth)
                {
                    catUnit.runtimeData.currentHealth = catUnit.runtimeData.maxHealth;
                }

                AudioManager.instance.PlaySFX("Heal");
                catUnit.OnHealthChange(catUnit.runtimeData.currentHealth, catUnit.runtimeData.maxHealth);
            }
        }
    }
}
