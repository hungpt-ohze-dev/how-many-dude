using DG.Tweening;
using UnityEngine;

public class UIShakeAnim : MonoBehaviour
{
    private Sequence seq;

    private void OnEnable()
    {
        DOShakeAnim();
    }

    private void DOShakeAnim()
    {
        seq?.Kill();
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector2.one * 1.05f, 0.15f).SetEase(Ease.OutQuad))
           .Append(transform.DOScale(Vector2.one, 0.15f).SetEase(Ease.InQuad))
           .SetLoops(-1, LoopType.Yoyo);
    }
}
