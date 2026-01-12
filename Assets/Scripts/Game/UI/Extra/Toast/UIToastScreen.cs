using Lean.Pool;
using UnityEngine;

public class UIToastScreen : BaseScreen
{
    [SerializeField] private UIToastItem item;
    [SerializeField] private Transform holder;

    public void ShowToast(string message)
    {
        UIToastItem toast = LeanPool.Spawn(item, holder);
        toast.ShowMessage(message);
    }

}
