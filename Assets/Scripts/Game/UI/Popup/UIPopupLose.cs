using com.homemade.modules.audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupLose : BasePopup
{
    [Header("Component")]
    [SerializeField] private RawImage preview;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        AudioController.Instance.PlaySound(SoundClips.lose);
    }

    public void OnReset()
    {
        MainGame.Instance.ResetLevel();
        Close();
    }

    public void OnHome()
    {
        MainGame.Instance.ReturnHome();
        Close();
    }
}
