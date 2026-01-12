using System;
using AssetKits.ParticleImage;
using UnityEngine;
using Lean.Pool;
using com.homemade.modules.audio;
using DG.Tweening;

public class UIExtraManager : MonoBehaviour
{
    [Header("Navigate")]
    [SerializeField] private UINavigateBar navigateBar;

    [Header("Resource")]
    [SerializeField] private UIResourceGroup resourceGroup;

    [Header("Trasition")]
    [SerializeField] private UITransitionScreen transition;

    [Header("Toast")]
    [SerializeField] private UIToastScreen toast;

    [Header("Coin Anim")]
    [SerializeField] private ParticleImage uiCoinFlyAnimPrefab;

    [Header("Tutorial")]
    [SerializeField] private GameObject maskTutObj;

    [Header("Combo")]
    [SerializeField] private ParticleImage uiStarFlyAnimPrefab;

    private UICoinInfo coinInfo;

    // Get set
    public UINavigateBar NavigateBar => navigateBar;
    public UITransitionScreen TransitionScreen => transition;
    public UIResourceGroup ResourceGroup => resourceGroup;

    private void Start()
    {
        //coinInfo = resourceGroup.GetResource<UICoinInfo>(ResourceType.Coin);

        //ShowHideNavigate(true);
    }

    public void ShowHideNavigate(bool show)
    {
        navigateBar.gameObject.SetActive(show);

        if(show)
        {
            navigateBar.SelectHome();
        }
    }

    public void ShowHideResource(bool show)
    {
        if(show)
        {
            resourceGroup.gameObject.SetActive(true);
            //resourceGroup.UpdateInfo();
        }
        else
        {
            resourceGroup.gameObject.SetActive(false);
        }
    }

    public void ShowToast(string message)
    {
        //toast.ShowToast(message);
    }

    public void ShowTransition()
    {
        transition.Show();
    }

    public void HideTransition()
    {
        transition.Hide();
    }

    public void ShowHideMaskTut(bool show)
    {
        maskTutObj.SetActive(show);
    }

    #region Fly Coin

    private Transform GetTarget(IconName icon)
    {
        Transform target = null;

        if(icon == IconName.Coin)
        {
            target = resourceGroup.GetIconTrans(icon);
        }
        else if(icon == IconName.Heart)
        {
            target = resourceGroup.GetIconTrans(icon);
        }
        else
        {
            //target = UIManager.Instance.GetActiveScreen<UIHomeScreen>().GetIconTrans(icon);
        }

        return target;
    }

    public void PlayCoinFlyAnim(IconName icon = IconName.Coin, Action callback = null)
    {
        var uiCoinFlyAnim = LeanPool.Spawn(uiCoinFlyAnimPrefab, transform);
        coinInfo.ResetCoinBeforeAdd();

        // Gán target -> Show
        Transform target = GetTargetCoinFly(icon);
        uiCoinFlyAnim.attractorTarget = target;
        uiCoinFlyAnim.gameObject.SetActive(true);

        // Lấy event particle cuối cùng bay đến vị trí target
        var animEvent = uiCoinFlyAnim.onLastParticleFinished;
        uiCoinFlyAnim.onAnyParticleFinished.AddListener(() =>
        {
            ReachTargetEffect(target);
            coinInfo.TextEffectOnPartical();
        });

        // Tạo anim event callback
        void listener()
        {
            coinInfo.EndTextEffect();
            callback?.Invoke();
            animEvent.RemoveListener(listener);

            uiCoinFlyAnim.onAnyParticleFinished.RemoveAllListeners();
            LeanPool.Despawn(uiCoinFlyAnim);
        }

        // Add callback
        animEvent.AddListener(listener);
        uiCoinFlyAnim.Play();

        // Sound
        //AudioController.Instance.PlaySound(SoundClips.coin_fly);
    }

    private Transform GetTargetCoinFly(IconName icon)
    {
        Transform target = GetTarget(icon);

        //if (DataManager.Save.PiggyBank.firstTimeShow)
        //{
        //    target = GetTarget(IconName.PiggyBank);
        //}

        return target;
    }

    private void ReachTargetEffect(Transform target)
    {
        target.transform.DOKill();
        target.transform.localScale = Vector3.one;

        Sequence s = DOTween.Sequence();
        s.Append(target.transform.DOScale(1.3f, 0.1f).SetEase(Ease.Linear));
        s.Append(target.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear));
    }

    #endregion
}
