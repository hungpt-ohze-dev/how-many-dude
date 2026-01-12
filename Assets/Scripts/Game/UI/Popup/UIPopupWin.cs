using AssetKits.ParticleImage;
using com.homemade.modules.audio;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupWin : BasePopup
{
    [Header("Component")]
    [SerializeField] private TMP_Text rewardTxt;
    [SerializeField] private RawImage preview;

    [Header("VFX")]
    [SerializeField] private ParticleImage coinFly;
    [SerializeField] private Transform coinTrans;

    public override void Open(object obj = null)
    {
        base.Open(obj);


        AudioController.Instance.PlaySound(SoundClips.win);

        coinFly.Play();
    }

    public void OnNext()
    {
        MainGame.Instance.NextLevel();
        Close();
    }

    public void OnHome()
    {
        MainGame.Instance.ReturnHome();
        Close();
    }

    public void CoinAnim()
    {
        coinTrans.DORestart();
        coinTrans.DOScale(Vector3.one * 1.2f, 0.2f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                coinTrans.DOScale(Vector3.one, 0.15f)
                    .SetEase(Ease.Linear);
            });
    }
}
