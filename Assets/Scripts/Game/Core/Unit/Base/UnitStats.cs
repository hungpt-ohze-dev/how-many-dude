using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();

    private void Awake()
    {
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            Stats[type] = new Stat();
        }
    }

    public float GetStat(StatType type)
    {
        return Stats[type].Value;
    }

    public void AddModifier(StatModifier modifier)
    {
        Stats[modifier.StatType].AddModifier(modifier);
    }

    public void RemoveModifierFromSource(object source)
    {
        foreach (var stat in Stats.Values)
        {
            stat.RemoveModifier(stat
                .Modifiers
                .Find(m => m.Source == source));
        }
    }
}
