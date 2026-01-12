using com.homemade.pattern.observer;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UIBoosterButton : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] protected BoosterType type;

    [Header("Component")]
    [SerializeField] protected GameObject blockObj;
    [SerializeField] protected TMP_Text blockTxt;
    [SerializeField] protected GameObject addObj;
    [SerializeField] protected GameObject numObj;
    [SerializeField] protected TMP_Text numTxt;

    //protected MapSave mapSave;
    protected ResourceSave resourceSave;
    protected BoosterData data;

    protected bool isActive;
    protected bool isWait = false;

    public BoosterType Type => type;
    public bool IsActive => isActive;
    public int Amount => resourceSave.boosters[type];

    private void Awake()
    {
        //mapSave = DataManager.Save.Map;
        resourceSave = DataManager.Save.Resource;
        data = DataManager.Config.Boosters.Find(b => b.type == type);

        this.RegisterListener(EventID.Update_Booster, UpdateBooster);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Update_Booster, UpdateBooster);
    }

    public virtual void Set()
    {
        //isActive = mapSave.levelId >= data.levelUnlock;
        blockObj.SetActive(!isActive);
        blockTxt.text = $"Unlock Lv {data.levelUnlock}";

        UpdateBooster();
    }

    protected virtual void UpdateBooster(object obj = null)
    {
        if(!isActive)
        {
            addObj.SetActive(false);
            numObj.SetActive(false);
            return;
        }    

        bool isHas = resourceSave.boosters[type] > 0;
        addObj.SetActive(!isHas);
        numObj.SetActive(isHas);
        numTxt.text = $"{resourceSave.boosters[type]}";
    }

    public virtual void OnClick()
    {
        // Check for active
        if (!isActive)
        {
            //UIManager.Extra.ShowToast("Not available");
            return;
        }

        // Check for using firegun
        //if (GamePlay.IsUsingFireGun)
        //{
        //    UIManager.Extra.ShowToast("Not available");
        //    return;
        //}

        // Show popup buy
        if (resourceSave.boosters[type] <= 0)
        {
            //UIManager.Instance.ShowPopup<UIPopupBuyBooster>(data);
            return;
        }

        // Disable input
        //if (!DragDropManager.InputStatus) return;

        // Check for spaming
        //if (isWait || GamePlay.IsUsingBooster)
        //{
        //    UIManager.Extra.ShowToast("Wait for cooldown");
        //    return;
        //}

        isWait = true;

        OnUseBooster();
        Invoke(nameof(ResetWait), DataManager.Config.BoosterDelay);
    }

    private void ResetWait()
    {
        isWait = false;
        //GamePlay.IsUsingBooster = false;
    }

    protected virtual void OnUseBooster()
    {
        //GamePlay.IsUsingBooster = true;
        resourceSave.SubtractBooster(type, 1);

        //GamePlay.CurrentLevel.Helper.TrackingBoosterUse(type);
    }

}
