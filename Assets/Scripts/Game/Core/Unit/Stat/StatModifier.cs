public enum StatModifierType
{
    Flat,        // +10 Attack
    PercentAdd  // +20% Attack
}

public class StatModifier
{
    public StatType StatType;
    public float Value;
    public StatModifierType Type;
    public object Source;

    public StatModifier(StatType statType, float value, StatModifierType type, object source)
    {
        StatType = statType;
        Value = value;
        Type = type;
        Source = source;
    }
}

[System.Serializable]
public class StatModifierData
{
    public StatType statType;
    public float value;
    public StatModifierType modifierType;
}
