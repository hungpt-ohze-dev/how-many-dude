using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;       // How fast zoom reacts
    public float minZoom = 2f;         // Minimum orthographic size
    public float maxZoom = 10f;        // Maximum orthographic size

    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam.orthographic == false)
        {
            Debug.LogWarning("This script is for Orthographic cameras only!");
        }

        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandlePCZoom();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleMobileZoom();
#endif

        // Smooth zoom transition
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        //cam.orthographicSize = targetZoom;
    }

    void HandlePCZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void HandleMobileZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Positions in previous frame
            Vector2 touchZeroPrev = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;

            // Distance between touches (current & previous)
            float prevMagnitude = (touchZeroPrev - touchOnePrev).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            targetZoom -= difference * 0.01f; // Scale factor for pinch sensitivity
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    public void SetBeginSize(float size)
    {
        targetZoom = size;
    }
}
