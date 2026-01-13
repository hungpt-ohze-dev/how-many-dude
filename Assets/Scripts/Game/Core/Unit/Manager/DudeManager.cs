using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeManager : MonoBehaviour
{
    [Header("Test")]
    public Transform target;

    public List<Dude> listDudes = new();

    private void Start()
    {
        foreach (var dude in listDudes)
        {
            dude.Init();
            dude.SetTarget(target.GetComponent<UnitBase>());
        }

    }

    public void UpdateState()
    {
        foreach (var dude in listDudes)
        {
            dude.UpdateState();
        }
    }
}
