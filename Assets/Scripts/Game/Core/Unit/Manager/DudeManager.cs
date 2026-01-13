using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeManager : MonoBehaviour
{
    [Header("Test")]
    public Transform target;

    public List<Dude> listDudes = new();

    public void Init()
    {
        foreach (var dude in listDudes)
        {
            dude.Init();
        }
    }

    public void UpdateState()
    {
        foreach (var dude in listDudes)
        {
            dude.UpdateState();
        }
    }

    public void StartAction()
    {
        foreach (var dude in listDudes)
        {
            dude.StartAction();
        }
    }

    public void StopAction()
    {
        foreach (var dude in listDudes)
        {
            dude.StopAction();
        }
    }
}
