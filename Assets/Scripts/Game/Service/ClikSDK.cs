//using Tabtale.TTPlugins;
using UnityEngine;

public class ClikSDK : MonoBehaviour
{
    public bool turnOn = true;

    private void Awake()
    {
        if (!turnOn) return;

        // Initialize CLIK Plugin
        //TTPCore.Setup();
    }
}
