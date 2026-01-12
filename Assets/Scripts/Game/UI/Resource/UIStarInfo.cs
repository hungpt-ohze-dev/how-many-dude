using UnityEngine;

public class UIStarInfo : UIResourceInfo
{
    public override void OnClick()
    {
        //if (!UIPopupCheat.IsCheater) return;

        save.Add(ResourceType.Star, 5);
    }

    public void OnAddStar()
    {
        //UIManager.Instance.ShowPopup<UIPopupAddStar>();
    }
}
