using UnityEngine;
using UnityEngine.UI;

public class UISettingItem : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Image toggleBGImg;
    [SerializeField] private Sprite toggleSprite_On;
    [SerializeField] private Sprite toggleSprite_Off;

    [Header("Icon")]
    [SerializeField] private RectTransform iconRT;

    private readonly float xPos = -30f;

    public void Set(bool isOn)
    {
        toggleBGImg.sprite = isOn ? toggleSprite_On : toggleSprite_Off;
        iconRT.anchoredPosition = isOn ? new Vector2(-xPos, 0f) : new Vector2(xPos, 0f);
    }

}
