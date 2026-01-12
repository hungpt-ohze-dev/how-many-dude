using DG.Tweening;
using UnityEngine;

public class UIButtonScaleAnim : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool useAnim = true;
    [SerializeField] private float scaleUpValue = 1.05f;
    [SerializeField] private float duration = 0.35f;

    private RectTransform rect;
    private Tween scaleTween;

    private Sequence clickSeq;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (!useAnim) return;

        scaleTween?.Kill();
        scaleTween = transform.DOScale(scaleUpValue, duration)
                              .SetLoops(-1, LoopType.Yoyo)
                              .SetEase(Ease.InOutQuad);
    }

    void OnDisable()
    {
        scaleTween?.Kill();
        clickSeq.Restart();
    }

    public void DoOnClickAnim()
    {
        float startY = rect.anchoredPosition.y;

        clickSeq = DOTween.Sequence();

        clickSeq.Append(rect.DOAnchorPosY(startY + 20f, 0.2f).SetEase(Ease.OutBack))
                .Append(rect.DOAnchorPosY(startY, 0.2f).SetEase(Ease.OutBack));
    }

    public void DoOnClickScaleAnim()
    {
        clickSeq = DOTween.Sequence();

        clickSeq.Append(transform.DOScale(Vector2.one * 1.2f, 0.15f).SetEase(Ease.OutBack))
                .Append(transform.DOScale(Vector2.one, 0.15f).SetEase(Ease.OutBack));
    }

}
