using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameScreen : BaseScreen
{
    [Header("Component")]
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private NumberTween iqTxt;

    [Header("Resource")]
    [SerializeField] private UICoinInfo coinInfo;

    private LevelSave levelSave;

    protected override void Init()
    {
        levelSave = DataManager.Save.Level;

        //coinInfo.Set();
    }

    public override void Show()
    {
        base.Show();

        //coinInfo.UpdateInfo();
        levelTxt.text = $"Level {levelSave.levelId}";
        //iqTxt.Current = 60f;
    }

    public void OnSetting()
    {
        UIManager.Instance.ShowPopup<UIPopupSetting>();
    }

    public void OnReset()
    {
        MainGame.Instance.ResetLevel();
    }

    public void UpdateIQ(float amount)
    {
        float value = iqTxt.Current + amount;
        iqTxt.AnimateNumber((int)value, 0.8f);
    }

    public void UpdateLevel()
    {
        levelTxt.text = $"Level {levelSave.levelId}";
    }
}
