using System;

[Serializable]
public class RewardData
{
    public RewardType type;
    public IRewardData data;

    public RewardData() {}

    public RewardData(string rewardString)
    {
        if (Enum.TryParse(rewardString.Split("_")[0], out RewardType rewardType))
        {
            type = rewardType;
            data = rewardType switch
            {
                RewardType.Resource => new ResourceReward(rewardString),
                RewardType.Booster => new BoosterReward(rewardString),
                _ => throw new ArgumentException("Invalid reward class")
            };
        }
    }

}
