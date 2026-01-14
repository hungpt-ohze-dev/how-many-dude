using com.homemade.pattern.singleton;
using com.homemade.tick;
using com.homemade.mec;
using UnityEngine;

public class TickManager : LiveSingleton<TickManager>
{
    private TickData gamePlayTick;
    private TickData combatTick;

    // Get set
    public static TickData GamePlayTick => Instance.gamePlayTick;
    public static TickData CombatTick => Instance.combatTick;

    protected override void OnInit()
    {
        base.OnInit();

        gamePlayTick = new TickData(Segment.Update);
        combatTick = new TickData(Segment.Update, 0.5f);
    }

}
