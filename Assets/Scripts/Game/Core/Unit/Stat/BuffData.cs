using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Buff Data")]
public class BuffData : ScriptableObject
{
    public string buffName;
    public BuffType buffType;
    public float duration;
    public BuffStackType stackType;

    public List<StatModifierData> statModifiers;

    public bool hasTickEffect;
    public float tickInterval; // mỗi bao lâu tick
    public float tickValue;    // damage / heal
}
