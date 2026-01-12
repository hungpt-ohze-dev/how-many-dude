using System;

[Serializable]
public class SettingSave : BaseDataSave
{
    public bool sound;
    public bool music;
    public bool vibrate;

    public override void Init()
    {
        base.Init();

        sound = true;
        music = true;
        vibrate = true;
    }
}
