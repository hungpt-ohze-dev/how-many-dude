using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public static class RewardUtils
{
    public static class RewardPlace
    {
        public static string DailyTask = "DailyTask";
        public static string StarterPack = "StarterPack";
        public static string DailyReward = "DailyReward";
        public static string GoldRush = "GoldRush";
        public static string GrowthFundFree = "GrowthFundFree";
        public static string GrowthFundPremium = "GrowthFundPremium";
    }

    public static List<RewardData> GetRewards(string dataString)
    {
        List<RewardData> rewards = new List<RewardData>();

        string[] dataSplited = dataString.Split('|');

        foreach (string data in dataSplited)
        {
            if (string.IsNullOrEmpty(data)) continue;

            RewardData reward = new RewardData(data);
            rewards.Add(reward);
        }

        return rewards;
    }

    public static TReward CastData<TReward>(RewardData reward) where TReward : IRewardData
    {
        return (TReward) reward.data;
    }

    //public static UIRewardItem GetRewardItem(RewardData reward, Transform holder, string where = "")
    //{
    //    string rewardType = $"{reward.type.ToString()}{where}";
    //    var rewardLoad = AddressableManager.Instance.LoadRewardItem(rewardType);
    //    UIRewardItem item = LeanPool.Spawn(rewardLoad, holder);
    //    return item;
    //}

    //private static void SaveReward(RewardData reward)
    //{
    //    ResourceSave save = DataManager.Save.Resource;

    //    if (reward.type == RewardType.Resource)
    //    {
    //        var rewardCast = CastData<ResourceReward>(reward);
    //        save.Add(rewardCast.resourceType, rewardCast.value);
    //    }
    //    else if (reward.type == RewardType.Booster)
    //    {
    //        var rewardCast = CastData<BoosterReward>(reward);
    //        save.AddBooster(rewardCast.boosterType, rewardCast.value);
    //    }
    //}

    public static void ReceiveReward(RewardData reward)
    {
        //SaveReward(reward);
    }

    public static void ReceiveRewards(List<RewardData> rewards)
    {
        foreach(var reward in rewards)
        {
            ReceiveReward(reward);
        }
    }
}
