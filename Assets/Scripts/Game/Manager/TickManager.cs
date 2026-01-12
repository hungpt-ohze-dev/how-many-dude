using com.homemade.pattern.singleton;
using com.homemade.tick;
using com.homemade.mec;
using UnityEngine;

public class TickManager : LiveSingleton<TickManager>
{
    private TickData gamePlayTick;

    // Get set
    public static TickData GamePlayTick => Instance.gamePlayTick;

    protected override void OnInit()
    {
        base.OnInit();

        gamePlayTick = new TickData(Segment.Update, 1f);
    }

}
