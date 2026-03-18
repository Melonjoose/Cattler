public enum StatType
{
    AttackSpeed,
    AttackPower,
    Range,
    Defense
}

public class Buff
{
    public string buffName;
    public float duration;
    public StatType statType;
    public float value;

    public Buff(string name, float dur, StatType type, float val)
    {
        buffName = name;
        duration = dur;
        statType = type;
        value = val;
    }

    public void Apply(CatUnit cat)
    {
        switch (statType)
        {
            case StatType.AttackSpeed:
                cat.runtimeData.attackSpeed += value;
                break;
            case StatType.AttackPower:
                cat.runtimeData.attackPower += (int)value;
                break;
            case StatType.Range:
                cat.runtimeData.attackRange += value;
                break;
        }
    }

    public void Remove(CatUnit cat)
    {
        switch (statType)
        {
            case StatType.AttackSpeed:
                cat.runtimeData.attackSpeed -= value;
                break;
            case StatType.AttackPower:
                cat.runtimeData.attackPower -= (int)value;
                break;
            case StatType.Range:
                cat.runtimeData.attackRange -= value;
                break;
        }
    }
}