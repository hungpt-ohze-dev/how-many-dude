using UnityEngine;
using TMPro;
using DG.Tweening;

public class NumberTween : MonoBehaviour
{
    public TMP_Text numberText;
    public string suffix = "";

    private Tween myTween;

    private float current;
    public float Current
    {
        get => current;
        set
        {
            current = value;
            SetText();
        }
    }

//#if UNITY_EDITOR
//    private void OnValidate()
//    {
//        numberText = GetComponent<TMP_Text>();
//    }
//#endif

    private void SetText()
    {
        KillAnim();
        numberText.text = numberText.text = $"{current:0}{suffix}";
    }

    public void AnimateNumber(int from, int to, float duration = 1f)
    {
        int value = from;
        myTween = DOTween.To(() => value, x => {
            value = x;
            numberText.text = value.ToString();
        }, to, duration);
    }

    public void AnimateNumber(int to, float duration = 1f)
    {
        myTween = DOTween.To(() => current, x =>
        {
            current = x;
            numberText.text = $"{current.ToString("0")}{suffix}";
        }, to, duration).SetEase(Ease.Linear); // You can change easing
    }

    public void KillAnim()
    {
        if (myTween != null && myTween.IsActive())
            myTween.Kill();
    }
}
