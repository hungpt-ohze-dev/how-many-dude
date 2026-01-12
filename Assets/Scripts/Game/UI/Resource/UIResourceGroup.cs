using System.Collections.Generic;
using UnityEngine;

public class UIResourceGroup : MonoBehaviour
{
    [Header("Resources Info")]
    [SerializeField] private List<UIResourceInfo> uIResources = new();

    public void Init()
    {
        foreach (var resource in uIResources)
        {
            resource.Set();
        }

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        foreach (var resource in uIResources)
        {
            resource.UpdateInfo();
        }
    }

    public void ShowResource(ResourceType type)
    {
        var uiR = uIResources.Find(r => r.Type == type);
        uiR.ShowHide(true);
    }

    public void HideResource(ResourceType type)
    {
        var uiR = uIResources.Find(r => r.Type == type);
        uiR.ShowHide(false);
    }

    public Transform GetIconTrans(IconName icon)
    {
        Transform target = null;

        if(icon == IconName.Coin)
        {
            target = uIResources.Find(r => r.Type == ResourceType.Coin).IconTrans;
        }
        else if(icon == IconName.Heart)
        {
            target = uIResources.Find(r => r.Type == ResourceType.Heart).IconTrans;
        }

        return target;
    }

    public T GetResource<T>(ResourceType type) where T : UIResourceInfo
    {
        uIResources.Find(r => r.Type == type);
        return (T)uIResources.Find(r => r.Type == type);
    }
}
