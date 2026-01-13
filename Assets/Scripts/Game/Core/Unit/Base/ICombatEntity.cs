using ProjectDawn.LocalAvoidance;
using UnityEngine;

public interface ICombatEntity
{
    bool IsDead { get; }
    Transform Transform { get; }
    Agent Agent { get; }

    void SetTarget(ICombatEntity target);
    ICombatEntity GetTarget();
}

