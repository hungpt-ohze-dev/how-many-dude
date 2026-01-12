using System;
using UnityEngine;

[Serializable]
public class ResourceReward : IRewardData
{
    public ResourceType resourceType;
    public int value;

    public ResourceReward(ResourceType resourceType, int value)
    {
        this.resourceType = resourceType;
        this.value = value;
    }

    public ResourceReward(string data)
    {
        if(Enum.TryParse(data.Split("_")[1], out ResourceType type))
        {
            resourceType = type;
        }

        if(int.TryParse(data.Split("_")[2], out int rs))
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
        return (ResourceReward) new(resourceType, value);
    }

    public void Log()
    {
        Debug.Log($"{resourceType}_{value}");
    }
}
