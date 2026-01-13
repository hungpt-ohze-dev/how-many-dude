using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemiesInGame = new();

    public List<Enemy> SpawnEnemies()
    {
        foreach(var e in enemiesInGame)
        {
            e.Init();
        }

        return enemiesInGame;
    }

    public void UpdateState()
    {
        foreach (var enemy in enemiesInGame)
        {
            enemy.UpdateState();
            enemy.UpdateBuff();
        }
    }

    public void StartAction()
    {
        foreach (var enemy in enemiesInGame)
        {
            enemy.StartAction();
        }
    }

    public void StopAction()
    {
        foreach (var enemy in enemiesInGame)
        {
            enemy.StopAction();
        }
    }
}
