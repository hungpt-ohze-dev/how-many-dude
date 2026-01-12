using com.homemade.pattern.singleton;
using com.homemade.tick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    [Header("Manager")]
    [SerializeField] private DudeManager dudeManager;
    [SerializeField] private EnemyManager enemyManager;

    // Static
    public DudeManager Dude => Instance.dudeManager;
    public EnemyManager Enemy => Instance.enemyManager;

    protected override void OnInit()
    {
        
    }

    protected override void Start()
    {
        TickManager.GamePlayTick.Action += GamePlayTick;
        TickManager.GamePlayTick.Register();

        enemyManager.SpawnEnemies();
    }

    private void GamePlayTick()
    {
        dudeManager.UpdateState();
    }
}
