using com.homemade.pattern.observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIResourceInfo : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected ResourceType type;
    [SerializeField] protected TMP_Text amountTxt;
    [SerializeField] protected Image icon;

    public ResourceType Type => type;
    public Transform IconTrans => icon.transform;

    protected ResourceSave save;
    protected GameConfig config;

    public int UpdateAmount{ get; set; }

    private void Awake()
    {
        this.RegisterListener(EventID.Update_Resource, OnUpdateResource);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.Update_Resource, OnUpdateResource);
    }

    private void OnUpdateResource(object obj = null)
    {
        UpdateAmount = (int)obj;

        UpdateInfo();
    }

    public virtual void Set()
    {
        save = DataManager.Save.Resource;
        config = DataManager.Config;
    }

    public virtual void UpdateInfo()
    {
        var amount = Utils.FormatWithSuffix(save.resouces[type]);

        amountTxt.text = amount; 
    }

    public abstract void OnClick();

    public void ShowHide(bool show)
    {
        gameObject.SetActive(show);
    }
}
