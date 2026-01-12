using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Image handIm;
    [SerializeField] private Sprite[] handSprites;
    [SerializeField] private Camera uiCamera;

    [Header("OnOff")]
    [SerializeField] private bool isOn;

    private void Start()
    {
        isOn = DataManager.Config.GamePlayShowHand;

#if !UNITY_EDITOR
        isOn = false;
#endif

        this.gameObject.SetActive(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn) return;

        if (Input.GetMouseButtonDown(0))
        {
            handIm.sprite = handSprites[0];
            handIm.SetNativeSize();
        }else if (Input.GetMouseButton(0))
        {
            handIm.sprite = handSprites[0];
            handIm.SetNativeSize();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            handIm.sprite = handSprites[1];
            handIm.SetNativeSize();
        }
        
        Vector3 pos = uiCamera.ScreenToWorldPoint(Input.mousePosition);
        handIm.transform.position = new Vector3(pos.x, pos.y, 0f);
    }
}
