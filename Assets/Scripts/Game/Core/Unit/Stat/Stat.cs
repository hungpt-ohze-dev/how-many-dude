using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    public float BaseValue;
    private List<StatModifier> modifiers = new List<StatModifier>();

    public List<StatModifier> Modifiers => modifiers;

    public float Value
    {
        get
        {
            float finalValue = BaseValue;
            float percentAdd = 0f;

            foreach (var mod in modifiers)
            {
                if (mod.Type == StatModifierType.Flat)
                    finalValue += mod.Value;
                else if (mod.Type == StatModifierType.PercentAdd)
                    percentAdd += mod.Value;
            }

            finalValue *= (1 + percentAdd);
            return finalValue;
        }
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(StatModifier mod)
    {
        modifiers.Remove(mod);
    }
}
