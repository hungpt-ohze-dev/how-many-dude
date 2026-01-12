using UnityEngine;
using DG.Tweening;

public class UISplashRotator : MonoBehaviour
{
    [SerializeField] private RectTransform splashImage;
    [SerializeField] private float rotationSpeed = 4f; // Time for one full rotation

    void Start()
    {
        // Make it rotate endlessly
        splashImage
            .DORotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear) // constant speed
            .SetLoops(-1, LoopType.Restart); // infinite loop
    }
}
