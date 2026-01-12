using com.homemade.pattern.observer;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceSave : BaseDataSave
{
    public int maxHeart = 5;
    public string timeOutGame;
    public int secondCountDown;
    public int secondNotSubtractHeart;

    [Title("Resource Info")]
    [ShowInInspector]
    public Dictionary<ResourceType, int> resouces = new();

    [Title("Booster type")]
    [ShowInInspector] 
    public Dictionary<BoosterType, int> boosters = new();

    // Get set
    public int Heart => resouces[ResourceType.Heart];
    public int Coin => resouces[ResourceType.Coin];
    public int Star => resouces[ResourceType.Star];


    public override void Init()
    {
        maxHeart = DataManager.Config.ResourceMaxHeart;
        timeOutGame = DateUtils.Now.ToString();
        secondCountDown = DataManager.Config.ResourceSecondAddHeart;
        secondNotSubtractHeart = 0;

        // Resource
        resouces = new()
        {
            { ResourceType.Heart, 5 },
            { ResourceType.Coin, 0 },
            { ResourceType.Star, 0 },
        };

        // Booster
        boosters = new()
        {
            { BoosterType.Package, 0 },
            { BoosterType.Refresh, 0 },
            { BoosterType.Time, 0 },
            { BoosterType.FireGun, 0 },
        };
    }

    public override void Fix()
    {
        OnComebackGame();
    }

    public void Add(ResourceType type, int amount)
    {
        resouces[type] += amount;

        Observer.Instance.PostEvent(EventID.Update_Resource, amount);
        Observer.Instance.PostEvent(EventID.PiggyBank_AddCoin, amount);
    }

    public void Subtract(ResourceType type, int amount)
    {
        if(type == ResourceType.Heart && secondNotSubtractHeart > 0)
        {
            return;
        }

        int newValue = Mathf.Clamp(resouces[type] - amount, 0, int.MaxValue);
        resouces[type] = newValue;

        Observer.Instance.PostEvent(EventID.Update_Resource, amount);
    }

    public void OnComebackGame()
    {
        DateTime lastTime = DateTime.Parse(timeOutGame);
        TimeSpan time = DateUtils.Now - lastTime;

        CheckAddHeart(time);
        CheckNotSubtractHeart(time);
    }

    private void CheckAddHeart(TimeSpan time)
    {
        if (Heart >= maxHeart)
        {
            secondCountDown = DataManager.Config.ResourceSecondAddHeart;
            return;
        }

        // Add heart when comeback game
        int secondAddHeart = DataManager.Config.ResourceSecondAddHeart;
        int heartAdd = (int)time.TotalSeconds / secondAddHeart;
        secondCountDown = secondCountDown - (int)time.TotalSeconds - (secondAddHeart * heartAdd);
        resouces[ResourceType.Heart] = Mathf.Clamp(Heart + heartAdd, 0, maxHeart);
    }

    private void CheckNotSubtractHeart(TimeSpan time)
    {
        // Time for not subtract heart
        secondNotSubtractHeart = secondNotSubtractHeart - (int)time.TotalSeconds;
        if(secondNotSubtractHeart < 0)
        {
            secondNotSubtractHeart = 0;
        }
    }

    public void OnOutGame()
    {
        timeOutGame = DateUtils.Now.ToString();
    }

    public void AddMaxHeart(int amount)
    {
        int max = DataManager.Config.HeartMax;
        maxHeart = Mathf.Clamp(maxHeart + amount, 0, max);
    }

    #region Booster
    public void AddBooster(BoosterType type, int amount)
    {
        boosters[type] += amount;

        Observer.Instance.PostEvent(EventID.Update_Booster);
    }

    public void SubtractBooster(BoosterType type, int amount)
    {
        int newValue = Mathf.Clamp(boosters[type] - amount, 0, int.MaxValue);
        boosters[type] = newValue;

        Observer.Instance.PostEvent(EventID.Update_Booster);
    }

    #endregion
}
