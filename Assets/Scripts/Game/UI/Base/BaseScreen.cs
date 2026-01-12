using UnityEngine;

public abstract class BaseScreen : MonoBehaviour, IScreen
{
    protected bool isInit = false;

    private void Awake()
    {
        if (isInit) return;

        isInit = true;
        Init();
    }

    protected virtual void Init()
    {
        
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
