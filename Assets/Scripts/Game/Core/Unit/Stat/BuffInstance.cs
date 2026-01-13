using System.Collections.Generic;

public class BuffInstance
{
    public BuffData Data;
    public float RemainingTime;
    public float TickTimer;

    private UnitStats stats;
    private UnitHealth health;

    private List<StatModifier> appliedModifiers = new();

    public BuffInstance(BuffData data, UnitStats stats, UnitHealth health)
    {
        Data = data;
        RemainingTime = data.duration;
        this.stats = stats;
        this.health = health;

        ApplyStatModifiers();
    }

    void ApplyStatModifiers()
    {
        foreach (var mod in Data.statModifiers)
        {
            var modifier = new StatModifier(
                mod.statType,
                mod.value,
                mod.modifierType,
                this
            );

            appliedModifiers.Add(modifier);
            stats.AddModifier(modifier);
        }
    }

    public void Update(float deltaTime)
    {
        RemainingTime -= deltaTime;

        if (!Data.hasTickEffect) return;

        TickTimer += deltaTime;
        if (TickTimer >= Data.tickInterval)
        {
            TickTimer = 0;
            health.ApplyDamage(Data.tickValue);
        }
    }

    public bool IsExpired => RemainingTime <= 0f;

    public void Remove()
    {
        foreach (var mod in appliedModifiers)
        {
            stats.RemoveModifierFromSource(this);
        }
    }
}
