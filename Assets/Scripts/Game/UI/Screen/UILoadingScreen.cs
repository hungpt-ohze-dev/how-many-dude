//using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : BaseScreen
{
    [Header("Container")]
    [SerializeField] private GameObject holder;
    [SerializeField] private AudioListener audioListener;
    //[SerializeField] private SkeletonGraphic sg;

    [Header("Loading bar")]
    [SerializeField] private Image loadingBarImg;

    private float timeLoading = 2f;

    private void Start()
    {
        Loading();
    }

    public override void Hide()
    {
        holder.SetActive(false);
    }

    private void Loading(Action onDone = null)
    {
        StartCoroutine(LoadingIEnum());

        IEnumerator LoadingIEnum()
        {
            loadingBarImg.fillAmount = 0;
            float progress = 0;

            while (progress < timeLoading)
            {
                progress += Time.deltaTime;
                loadingBarImg.fillAmount = progress / timeLoading;
                yield return null;
            }

            audioListener.enabled = false;
            //sg.AnimationState.GetCurrent(0).TimeScale = 0;

            yield return new WaitForEndOfFrame();
            onDone?.Invoke();
            OnLoadingDone();
        }
    }

    private void OnLoadingDone()
    {
        MonoScene.Instance.LoadMainScene(Hide);
    }
}
