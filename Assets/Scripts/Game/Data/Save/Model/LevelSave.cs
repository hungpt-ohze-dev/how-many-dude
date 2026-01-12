using System;
using UnityEngine;

[Serializable]
public class LevelSave : BaseDataSave
{
    public int levelId;

    public override void Init()
    {
        base.Init();

        levelId = 1;
    }

    public void FinishLevel()
    {
        levelId += 1;
        Save();
    }
}
