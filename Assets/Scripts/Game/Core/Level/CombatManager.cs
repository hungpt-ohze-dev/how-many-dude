using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Ally")]
    public List<UnitBase> allies = new List<UnitBase>();

    [Header("Enemy")]
    public List<UnitBase> enemies = new List<UnitBase>();

    public void CheckUnit()
    {
        AssignTargets(allies.Cast<ICombatEntity>().ToList(),
                      enemies.Cast<ICombatEntity>().ToList());

        AssignTargets(enemies.Cast<ICombatEntity>().ToList(),
                      allies.Cast<ICombatEntity>().ToList());
    }

    void AssignTargets(List<ICombatEntity> attackers, List<ICombatEntity> targets)
    {
        attackers = attackers.Where(a => a != null && !a.IsDead).ToList();
        targets = targets.Where(t => t != null && !t.IsDead).ToList();

        if (targets.Count == 0) return;

        Dictionary<ICombatEntity, int> targetCount = new Dictionary<ICombatEntity, int>();
        foreach (var t in targets)
            targetCount[t] = 0;

        foreach (var attacker in attackers)
        {
            // Nếu target hiện tại chết → reset
            if (attacker.GetTarget() != null && attacker.GetTarget().IsDead)
                attacker.SetTarget(null);

            // Nếu đã có target sống → giữ nguyên (tránh đổi target liên tục)
            if (attacker.GetTarget() != null)
            {
                targetCount[attacker.GetTarget()]++;
                continue;
            }

            ICombatEntity bestTarget = null;
            float bestScore = float.MaxValue;

            foreach (var target in targets)
            {
                float distance = Vector3.Distance(
                    attacker.Transform.position,
                    target.Transform.position);

                int count = targetCount[target];

                // Trọng số: mỗi attacker thêm = +5 điểm
                float score = count * 5f + distance;

                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = target;
                }
            }

            if (bestTarget != null)
            {
                attacker.SetTarget(bestTarget);
                targetCount[bestTarget]++;
            }
        }
    }
}
