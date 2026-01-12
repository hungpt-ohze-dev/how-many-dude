using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeScreen : BaseScreen
{
    [Header("Component")]
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private UICoinInfo coinInfo;
    [SerializeField] private RawImage preview;

    private LevelSave levelSave;

    protected override void Init()
    {
        levelSave = DataManager.Save.Level;

        coinInfo.Set();
    }

    public override void Show()
    {
        base.Show();

        levelTxt.text = $"Level {levelSave.levelId}";
        coinInfo.UpdateInfo();

    }

    public void OnPlay()
    {
        MainGame.Instance.LoadGame();
    }

    public void OnSetting()
    {
        UIManager.Instance.ShowPopup<UIPopupSetting>();
    }
}
