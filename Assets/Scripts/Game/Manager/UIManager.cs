using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Canvas")]
    [SerializeField] private Canvas screenCanvas;
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private GameObject popupBG;

    [Header("Extra")]
    [SerializeField] private UIExtraManager extraManager;
    public static UIExtraManager Extra => Instance.extraManager;

    private Dictionary<string, BaseScreen> screens = new Dictionary<string, BaseScreen>();
    private Dictionary<string, BasePopup> popups = new Dictionary<string, BasePopup>();

    private Dictionary<string, AsyncOperationHandle<GameObject>> screenHandles = new();
    private Dictionary<string, AsyncOperationHandle<GameObject>> popupHandles = new();

    private BaseScreen currentScreen;
    public BaseScreen CurrentScreen => currentScreen;

    public event Action PopupShowAction;
    public event Action PopupHideAction;
    public event Action PopupHideAllAction;

    #region Screen
    public async UniTask<TScreen> ShowScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        if (!screens.ContainsKey(nameScreen))
        {
            await CreateScreen<TScreen>();
        }

        // Close all screen and popup
        if (currentScreen != null)
        {
            currentScreen.Hide();
        }
        CloseAllPopup();

        currentScreen = GetScreen<TScreen>();
        currentScreen.transform.SetAsLastSibling();
        currentScreen.Show();

        return (TScreen)currentScreen;
    }

    private async UniTask<GameObject> LoadScreenAsync(string prefabAddress)
    {
        GameObject prefab;

        if (screenHandles.ContainsKey(prefabAddress))
        {
            // Already cached
            prefab = screenHandles[prefabAddress].Result;
        }
        else
        {
            // Load prefab asynchronously
            var handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new System.Exception($"Failed to load prefab: {prefabAddress}");

            prefab = handle.Result;
            screenHandles[prefabAddress] = handle;
        }

        return prefab;
    }

    private async UniTask CreateScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        string prefabAsset = $"Screen/{nameScreen}.prefab";

        GameObject cache = await LoadScreenAsync(prefabAsset);
        GameObject screenObj = Instantiate(cache, screenCanvas.transform);
        TScreen script = screenObj.GetComponent<TScreen>();
        screens.Add(nameScreen, script);
    }

    private TScreen GetScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        var sc = screens[nameScreen].GetComponent<TScreen>();
        sc.transform.SetAsLastSibling();
        return sc;
    }

    public TScreen GetActiveScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        var sc = screens[nameScreen].GetComponent<TScreen>();
        return sc;
    }

    public void DeleteScreen<TScreen>() where TScreen : BaseScreen
    {
        string nameScreen = typeof(TScreen).Name;
        if (screens.ContainsKey(nameScreen))
        {
            screens.Remove(nameScreen);
        }
        else
        {
            Debug.Log($"{nameScreen} is not exsits");
        }
    }

    #endregion

    #region Popup

    private async UniTask<GameObject> LoadPopupAsync(string prefabAddress)
    {
        GameObject prefab;

        if (popupHandles.ContainsKey(prefabAddress))
        {
            // Already cached
            prefab = popupHandles[prefabAddress].Result;
        }
        else
        {
            // Load prefab asynchronously
            var handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new System.Exception($"Failed to load prefab: {prefabAddress}");

            prefab = handle.Result;
            popupHandles[prefabAddress] = handle;
        }

        return prefab;
    }

    private async UniTask<TPopup> CreatePopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;
        string prefabAsset = $"Popup/{popupName}.prefab";

        GameObject cache = await LoadPopupAsync(prefabAsset);
        GameObject popupObj = Instantiate(cache, popupCanvas.transform);
        TPopup popup = popupObj.GetComponent<TPopup>();
        popups.Add(popupName, popup);

        return popup;
    }

    public async UniTask<TPopup> ShowPopup<TPopup>(object obj = null) where TPopup : BasePopup
    {
        popupBG.SetActive(true);
        //currentPopup?.DeActive();

        string popupName = typeof(TPopup).Name;
        BasePopup popup = null;

        if (!popups.ContainsKey(popupName))
        {
            popup = await CreatePopup<TPopup>();
        }
        else
        {
            popup = popups[popupName];
        }

        popup.transform.SetAsLastSibling();
        popup.Open(obj);

        PopupShowAction?.Invoke();

        return (TPopup) popup;
    }

    public TPopup GetPopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;

        if (popups.ContainsKey(popupName))
        {
            return (TPopup)popups[popupName];
        }

        return null;
    }

    public void ClosePopup<TPopup>() where TPopup : BasePopup
    {
        string popupName = typeof(TPopup).Name;

        if (!popups.ContainsKey(popupName)) return;

        popups[popupName].Close();
    }

    public void CloseAllPopup()
    {
        if (popups.Count == 0) return;

        foreach (var p in popups)
        {
            p.Value.DeActive();
        }

        HidePopupBG();
    }

    public void HidePopupBG()
    {
        PopupHideAction?.Invoke();

        if (!popups.Any(p => p.Value.isActiveAndEnabled))
        {
            popupBG.SetActive(false);

            PopupHideAllAction?.Invoke();
        }
    }

    public bool CheckPopupShowing()
    {
        return popups.Any(p => p.Value.isActiveAndEnabled);
    }

    public void CloseCurrentPopup()
    {
        Transform lastPopupTrans = popupCanvas.transform.GetChild(popupCanvas.transform.childCount - 1);
        if (lastPopupTrans != null)
        {
            lastPopupTrans.GetComponent<BasePopup>().Close();

            MoveLastSiblingPopup(lastPopupTrans);
        }
    }

    public void MoveLastSiblingPopup(Transform popup)
    {
        popup.SetSiblingIndex(1);
        popupBG.transform.SetAsFirstSibling();
    }

    #endregion

    #region Release

    public void ReleaseAllScreen()
    {
        foreach(var kvp in screens)
        {
            Destroy(kvp.Value.gameObject);
        }
        screens.Clear();

        foreach(var kvp in screenHandles)
        {
            Addressables.Release(kvp.Value);
        }
        screenHandles.Clear();
    }

    public void ReleaseAllPopup()
    {
        foreach (var kvp in popups)
        {
            Destroy(kvp.Value.gameObject);
        }
        popups.Clear();

        foreach (var kvp in popupHandles)
        {
            Addressables.Release(kvp.Value);
        }
        popupHandles.Clear();
    }

    #endregion
}
