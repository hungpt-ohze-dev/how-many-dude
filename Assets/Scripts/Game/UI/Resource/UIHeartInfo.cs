using System;
using TMPro;
using UnityEngine;

public class UIHeartInfo : UIResourceInfo
{
    [Header("Time")]
    [SerializeField] private TMP_Text timeTxt;

    [Header("Component")]
    [SerializeField] private GameObject addObj;
    [SerializeField] private GameObject infiniteHeart;
    [SerializeField] private TMP_Text timeInfiniteTxt;


    //private TickSystem heartTick;
    private int secondCountDown;
    private int notSubtractCountDown;

    public override void Set()
    {
        base.Set();

        //secondCountDown = MainGame.Instance.SecondCountDown;
        //heartTick = TickManager.Instance.GetTickSystem(TickID.Heart_Tick_ID);
        //heartTick.OnTick += OnHeartTick;
        //heartTick.OnTick += OnNotSubtractTick;

        //MainGame.Instance.AddHeartEvent += UpdateInfo;

        // First time appear
        if (save.Heart >= save.maxHeart)
        {
            timeTxt.text = $"Full";
        }
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(secondCountDown);
            timeTxt.text = $"{time.ShowTime()}";
        }
    }

    private void OnHeartTick(int count)
    {
        secondCountDown = save.secondCountDown;

        if (save.Heart >= save.maxHeart)
        {
            timeTxt.text = $"Full";
            return;
        }

        TimeSpan time = TimeSpan.FromSeconds(secondCountDown);
        timeTxt.text = $"{time.ShowTime()}";
    }

    private void OnNotSubtractTick(int count)
    {
        notSubtractCountDown = save.secondNotSubtractHeart;

        if (notSubtractCountDown <= 0)
        {
            infiniteHeart.SetActive(false);
            return;
        }

        TimeSpan time = TimeSpan.FromSeconds(notSubtractCountDown);
        timeInfiniteTxt.text = $"{time.ShowTime()}";
        infiniteHeart.SetActive(true);
    }

    public override void UpdateInfo()
    {
        base.UpdateInfo();

        //secondCountDown = MainGame.Instance.SecondCountDown;

        //bool showAdd = save.Heart < save.maxHeart && MainGame.Instance.GameMode == GameMode.HomeGame;
        //addObj.SetActive(showAdd);
    }

    public override void OnClick()
    {
        //UIManager.Instance.ShowPopup<UIPopupHeartInfo>();
    }
}
