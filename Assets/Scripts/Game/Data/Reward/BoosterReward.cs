using System;
using UnityEngine;

[Serializable]
public class BoosterReward : IRewardData
{
    public BoosterType boosterType;
    public int value;

    public BoosterReward(BoosterType boosterType, int value)
    {
        this.boosterType = boosterType;
        this.value = value;
    }

    public BoosterReward(string data)
    {
        if (Enum.TryParse(data.Split("_")[1], out BoosterType type))
        {
            boosterType = type;
        }

        if (int.TryParse(data.Split("_")[2], out int rs))
        {
            value = rs;
        }
    }

    public void Add(int amount)
    {
        value += amount;
    }

    public void Multiply(int mul)
    {
        value *= mul;
    }

    public IRewardData Clone()
    {
        return (BoosterReward) new(boosterType, value);
    }

    public void Log()
    {
        Debug.Log($"{boosterType}_{value}");
    }
}
