using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [Header("Component")]
    public UnitStats stats;
    public UnitHealth health;

    private List<BuffInstance> activeBuffs = new();

    public void UpdateBuff()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].Update(Time.deltaTime);

            if (activeBuffs[i].IsExpired)
            {
                activeBuffs[i].Remove();
                activeBuffs.RemoveAt(i);
            }
        }
    }

    public void AddBuff(BuffData data)
    {
        BuffInstance existing = activeBuffs.Find(b => b.Data == data);

        if (existing != null)
        {
            HandleStacking(existing, data);
            return;
        }

        activeBuffs.Add(new BuffInstance(data, stats, health));
    }

    void HandleStacking(BuffInstance existing, BuffData data)
    {
        switch (data.stackType)
        {
            case BuffStackType.RefreshDuration:
                existing.RemainingTime = data.duration;
                break;

            case BuffStackType.Stack:
                activeBuffs.Add(new BuffInstance(data, stats, health));
                break;

            case BuffStackType.Ignore:
                break;
        }
    }
}
