using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotifyHandler : MonoBehaviour
{
    public static Action OnChangeNotificationStatus = () => { };
    
    [SerializeField] private GameObject notificationGo;
    [SerializeField] private List<BaseNotifyHandler> notifySo;

    private void OnEnable()
    {
        OnChangeNotificationStatus += OnChangeNotifyStatus;
        notificationGo.SetActive(NeedShowNotification());
    }

    private void OnDisable()
    {
        OnChangeNotificationStatus -= OnChangeNotifyStatus;
    }

    private void OnChangeNotifyStatus()
    {
        notificationGo.SetActive(NeedShowNotification());
    }

    private bool NeedShowNotification()
    {
        return notifySo.Any(s => s.NeedShowNotification());
    }

    public static void InvokeStatus()
    {
        OnChangeNotificationStatus?.Invoke();
    }    
}
