using com.homemade.modules.audio;
using TMPro;
using UnityEngine;

public class UIPopupSetting : BasePopup
{
    [Header("Setting")]
    [SerializeField] private UISettingItem soundItem;
    [SerializeField] private UISettingItem musicItem;
    [SerializeField] private UISettingItem vibrateItem;

    [Header("Tool")]
    [SerializeField] private TMP_Text versionTxt;

    private SettingSave settingSave;

    private void Awake()
    {
        settingSave = DataManager.Save.Setting;
    }

    public override void Open(object obj = null)
    {
        base.Open(obj);
        SetSetting();

        versionTxt.text = $"v{Application.version}";
    }

    private void SetSetting()
    {
        soundItem.Set(settingSave.sound);
        musicItem.Set(settingSave.music);
        //vibrateItem.Set(settingSave.vibrate);
    }

    public void OnChangeSound()
    {
        bool newValue = !settingSave.sound;
        AudioController.Instance.TurnOnOffSound(newValue);

        settingSave.sound = newValue;
        settingSave.Save();

        soundItem.Set(newValue);
    }

    public void OnChangeMusic()
    {
        bool newValue = !settingSave.music;
        AudioController.Instance.TurnOnOffMusic(!newValue);

        settingSave.music = newValue;
        settingSave.Save();

        musicItem.Set(newValue);
    }

    public void OnChangeVibrate()
    {
        bool newValue = !settingSave.vibrate;
        VibrationManager.IsVibrate = newValue;

        settingSave.vibrate = newValue;
        settingSave.Save();

        vibrateItem.Set(newValue);
    }
}
