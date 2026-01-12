using System;
using UnityEngine;

public class UINavigateBar : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private UINavigateButton[] buttons;

    private int selectedIndex = 0;

    public UINavigateButton MapButton => buttons[1];

    private void OnButtonClicked(int index, Action callback = null)
    {
        if(selectedIndex == index)
        {
            return;
        }    

        selectedIndex = index;
        UpdateSelection();

        callback?.Invoke();
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedIndex)
            {
                buttons[i].Selection();
            }
            else
            {
                buttons[i].DeSelection();
            }
        }
    }

    public void SelectShop()
    {
        Action callback = () =>
        {
            //UIManager.Instance.ShowScreen<UIShopScreen>();
        };

        //OnButtonClicked(0, callback);

        UIManager.Extra.ShowToast("Comming soon");
    }    

    public void SelectMap()
    {
        //if(DataManager.Save.Map.levelId < 5)
        //{
        //    UIManager.Extra.ShowToast("Reach level 5 to unlock");
        //    return;
        //}

        Action callback = () =>
        {
            //MainGame.Instance.DecorMode();
        };

        //OnButtonClicked(1, callback);
    }

    public void SelectHome()
    {
        Action callback = () =>
        {
            //if (MainGame.Instance.GameMode == GameMode.Decor)
            //{
            //    MainGame.Instance.DoneDecorMode();
            //}
            //else
            //{
            //    UIManager.Instance.ShowScreen<UIHomeScreen>();
            //}
        };

        //OnButtonClicked(1, callback);
    }

    public void SelectGallery()
    {
        Action callback = () =>
        {
            //UIManager.Instance.ShowScreen<UIGalleryScreen>();
        };

        //OnButtonClicked(3, callback);

        UIManager.Extra.ShowToast("Comming soon");
    }

    public void SelectRank()
    {
        //UIManager.Extra.ShowToast("Comming soon!");
    }
}
