using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Info")]
    public BattleResult battleResult = BattleResult.None;

    [Header("Ally")]
    public List<UnitBase> allies = new List<UnitBase>();

    [Header("Enemy")]
    public List<UnitBase> enemies = new List<UnitBase>();

    private bool isDone = false;

    public void Setup()
    {
        isDone = false;
    }

    public void CheckUnit()
    {
        //if (isDone) return;

        AssignTargets(allies.Cast<ICombatEntity>().ToList(),
                      enemies.Cast<ICombatEntity>().ToList());

        AssignTargets(enemies.Cast<ICombatEntity>().ToList(),
                      allies.Cast<ICombatEntity>().ToList());

        CheckBattleResult();
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

    void ClearTargets(List<ICombatEntity> attackers)
    {
        foreach (var a in attackers)
        {
            a.SetTarget(null);
            a.Done();
        }
    }

    //============================ Result ============================
    void CheckBattleResult()
    {
        if (battleResult != BattleResult.None)
            return; // Đã có kết quả → không check nữa

        isDone = true;

        bool allyAlive = allies.Any(a => a != null && !a.IsDead);
        bool enemyAlive = enemies.Any(e => e != null && !e.IsDead);

        if (!enemyAlive && allyAlive)
        {
            battleResult = BattleResult.AllyWin;
            OnAllyWin();
        }
        else if (!allyAlive && enemyAlive)
        {
            battleResult = BattleResult.EnemyWin;
            OnEnemyWin();
        }
    }

    private void OnAllyWin()
    {
        Debug.Log("ALLY WIN");

        // Clear target để dừng đánh
        ClearTargets(allies.Cast<ICombatEntity>().ToList());

        // TODO:
        // - Play victory animation
        // - Drop reward
    }

    private void OnEnemyWin()
    {
        Debug.Log("ENEMY WIN");

        ClearTargets(enemies.Cast<ICombatEntity>().ToList());

        // TODO:
        // - Game Over
    }

}
