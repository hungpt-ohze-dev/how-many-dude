using Cysharp.Threading.Tasks;
using UnityEngine;

public class UICoinInfo : UIResourceInfo
{
    private int resetAmount = 0;

    public override void Set()
    {
        base.Set();
    }

    public override void OnClick()
    {
        //if (!UIPopupCheat.IsCheater) return;

        save.Add(ResourceType.Coin, 100);
    }

    public void ShowShop()
    {
        //UIManager.Extra.NavigateBar.SelectShop();
    }

    public void ResetCoinBeforeAdd()
    {
        resetAmount = save.Coin - UpdateAmount;
        SetText(resetAmount);
    }

    public void SetText(int amount)
    {
        var stringAmount = Utils.FormatWithSuffix(amount);
        amountTxt.text = stringAmount;
    }  
    
    public void TextEffectOnPartical()
    {
        int add = UpdateAmount / 6;
        resetAmount += add;
        SetText(resetAmount);
    }

    public void EndTextEffect()
    {
        SetText(save.Coin);
    }
}
