using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationScalePopup : MonoBehaviour
{
    [Header("Animation Config")]
    [SerializeField] private float startSize = 0.7f, middleSize = 1.05f, endSize = 1;
    [SerializeField] private float firstTime = 0.2f, secondTime = 0.1f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform content;

    Sequence sequence;

    public void Open(Action callback = null)
    {
        content.transform.localScale = Vector3.one * startSize;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(content.transform.DOScale(middleSize, firstTime))
                .Join(canvasGroup.DOFade(1, firstTime))
                .Append(content.transform.DOScale(endSize, secondTime)
                .OnComplete(() => callback?.Invoke()));
    }

    public void Close(Action callback = null)
    {
        content.transform.localScale = Vector3.one * endSize;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(content.transform.DOScale(middleSize, secondTime).SetEase(Ease.OutBack))
                .Append(content.transform.DOScale(startSize + 0.1f, firstTime))
                .Join(canvasGroup.DOFade(0, firstTime))
                .OnComplete(() => callback?.Invoke());
    }
}
