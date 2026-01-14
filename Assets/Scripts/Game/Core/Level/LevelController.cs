using com.homemade.pattern.singleton;
using com.homemade.tick;
using ProjectDawn.LocalAvoidance.Demo;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    [Header("Manager")]
    [SerializeField] private AgentSystem agentSystem;
    [SerializeField] private DudeManager dudeManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CombatManager combatManager;

    // Static
    public AgentSystem AgentSystem => agentSystem;
    public DudeManager Dude => Instance.dudeManager;
    public EnemyManager Enemy => Instance.enemyManager;
    public CombatManager Combat => Instance.combatManager;

    // Private
    private bool isPlaying = false;

    protected override void OnInit()
    {
        Application.targetFrameRate = 60;
    }

    protected override void Start()
    {
        // Agent
        agentSystem.enabled = false;

        // Tick
        TickManager.GamePlayTick.Action += GamePlayTick;
        TickManager.GamePlayTick.Register();
        TickManager.CombatTick.Action += CombatTick;
        TickManager.CombatTick.Register();

        // Init
        var enemies = enemyManager.SpawnEnemies();
        dudeManager.Init();

        combatManager.allies.AddRange(dudeManager.listDudes);
        combatManager.enemies.AddRange(enemies);
        combatManager.Setup();
    }

    // ======================= Tick ============================
    private void GamePlayTick()
    {
        if (!isPlaying) return;

        dudeManager.UpdateState();
        enemyManager.UpdateState();
    }

    private void CombatTick()
    {
        combatManager.CheckUnit();
    }

    // ======================= Basic ==========================
    [Button]
    public void StartLevel()
    {
        isPlaying = true;
        agentSystem.enabled = true;

        dudeManager.StartAction();
        enemyManager.StartAction();
    }

    [Button]
    public void LevelDone()
    {
        isPlaying = false;
        agentSystem.enabled = false;

        dudeManager.StopAction();
        enemyManager.StopAction();
    }
}
