using System;
using UnityEngine;

[Serializable]
public class UserSave : BaseDataSave
{
    public string userId;
    public string deviceId;

    public bool isNewUser;
    public int session;

    public string lastPlayTime;

    public override void Init()
    {
        base.Init();

        userId = Guid.NewGuid().ToString();

        deviceId = SystemInfo.deviceUniqueIdentifier;

        isNewUser = true;
        session = 0;
    }

    public override void Fix()
    {
        isNewUser = false;
        session += 1;
    }

    public override void NewDay()
    {
        session = 0;
    }

    public override void Clear()
    {
        base.Clear();

        userId = string.Empty;
        deviceId = string.Empty;
        isNewUser = false;
    }

    public void FirstTimePlayGame()
    {
        isNewUser = false;
        Save();
    }
}
