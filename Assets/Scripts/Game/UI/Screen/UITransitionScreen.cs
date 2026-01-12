using Cysharp.Threading.Tasks;
using DG.Tweening;
//using Spine.Unity;
using UnityEngine;

public class UITransitionScreen : BaseScreen
{
    [Header("Content")]
    [SerializeField] private RectTransform mask;

    [Header("Target size")]
    [SerializeField] private Vector2 targetOutSizeDelta = new(4000, 4000);
    [SerializeField] private Vector2 targetInSizeDelta = Vector2.zero;

    [Header("Old Transition")]
    [SerializeField] private GameObject oldTransObj;

    //[Header("Anim")]
    //[SerializeField] private SkeletonGraphic sg;

    private int count = 0;
    private readonly float transitionTime = 0.5f;

    public override void Show()
    {
        base.Show();
    }

    public async override void Hide()
    {
        if (count == 0)
        {
            count++;
            await UniTask.WaitForSeconds(1f);
            oldTransObj.SetActive(false);
            base.Hide();
            return;
        }
        else
        {
            TransitionOut();
        }
    }

    public async UniTask TransitionIn()
    {
        base.Show();
        if (count == 0)
        {
            //sg.gameObject.SetActive(false);
            oldTransObj.SetActive(true);
            return;
        }

        mask.DOKill();
        mask.sizeDelta = targetOutSizeDelta;

        mask.DOSizeDelta(targetInSizeDelta, transitionTime)
                    .SetUpdate(true)
                    .SetEase(Ease.Linear);

        await UniTask.WaitForSeconds(transitionTime);

        // Anim
        //sg.transform.localScale = Vector3.zero;
        //sg.gameObject.SetActive(true);
        //sg.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        await UniTask.WaitForSeconds(1f);

        await UniTask.CompletedTask;
    }

    public async void TransitionOut()
    {
        mask.DOKill();
        mask.sizeDelta = targetInSizeDelta;

        //sg.transform.DOScale(0f, 0.25f).SetEase(Ease.InSine)
        //    .OnComplete(() =>
        //    {
        //        sg.gameObject.SetActive(false);
        //    });

        await UniTask.WaitForSeconds(transitionTime);
        mask.DOSizeDelta(targetOutSizeDelta, transitionTime)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                base.Hide();
            });
    }
}
