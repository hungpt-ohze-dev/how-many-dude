using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINavigateButton : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private GameObject highlightObj;

    [Header("Layer")]
    [SerializeField] private LayoutElement layout;

    public void Selection()
    {
        highlightObj.SetActive(true);

        layout.flexibleWidth = 1f;
        DOTween.To(
            () => layout.flexibleWidth,
            x => layout.flexibleWidth = x, 1f, 0.25f);

        icon.transform.DOScale(1f, 0.25f)
            .SetEase(Ease.OutSine);

        nameTxt.gameObject.SetActive(true);
    }   
    
    public void DeSelection()
    {
        highlightObj.SetActive(false);

        layout.flexibleWidth = 1f;
        icon.transform.DOScale(0.6f, 0.25f)
            .SetEase(Ease.OutSine);

        nameTxt.gameObject.SetActive(false);
    } 
}
