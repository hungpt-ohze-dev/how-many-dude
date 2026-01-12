using com.homemade.modules.audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtonSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string soundName = "tap_btn";

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.Instance.PlaySound(soundName);
    }
}
