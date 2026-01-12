using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIAutoScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public float slideDuration = 0.5f;
    public float waitTime = 3f;
    public float snapSpeed = 10f;

    [Header("Dots")]
    public Transform dotsContainer;    // Parent holding the dots
    public Color normalDotColor = Color.gray;
    public Color activeDotColor = Color.white;

    private int totalSlides;
    private int currentIndex = 0;
    private bool isDragging = false;
    private Coroutine autoSlideRoutine;
    private float resumeTimer = 0f;
    private Image[] dots;

    void Start()
    {
        totalSlides = scrollRect.content.childCount;
        dots = new Image[dotsContainer.childCount];

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = dotsContainer.GetChild(i).GetComponent<Image>();
        }

        UpdateDots();
        autoSlideRoutine = StartCoroutine(AutoSlide());
    }

    void Update()
    {
        if (!isDragging && resumeTimer > 0f)
        {
            resumeTimer -= Time.deltaTime;
            if (resumeTimer <= 0f && autoSlideRoutine == null)
            {
                autoSlideRoutine = StartCoroutine(AutoSlide());
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        StopAutoSlide();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        resumeTimer = 1f;
        SnapToNearestPanel();
    }

    IEnumerator AutoSlide()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            currentIndex = (currentIndex + 1) % totalSlides;
            float target = (float)currentIndex / (totalSlides - 1);
            yield return StartCoroutine(SmoothScroll(target, slideDuration));
            UpdateDots();
        }
    }

    IEnumerator SmoothScroll(float target, float duration)
    {
        float start = scrollRect.horizontalNormalizedPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = target;
    }

    void SnapToNearestPanel()
    {
        float pos = scrollRect.horizontalNormalizedPosition;
        float step = 1f / (totalSlides - 1);
        int nearestIndex = Mathf.RoundToInt(pos / step);
        currentIndex = Mathf.Clamp(nearestIndex, 0, totalSlides - 1);

        StopAllCoroutines();
        StartCoroutine(SmoothScroll((float)currentIndex / (totalSlides - 1), 1f / snapSpeed));
        UpdateDots();
    }

    void StopAutoSlide()
    {
        if (autoSlideRoutine != null)
        {
            StopCoroutine(autoSlideRoutine);
            autoSlideRoutine = null;
        }
    }

    void UpdateDots()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = (i == currentIndex) ? activeDotColor : normalDotColor;
        }
    }
}
