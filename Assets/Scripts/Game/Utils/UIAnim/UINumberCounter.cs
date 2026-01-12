using System.Collections;
using TMPro;
using UnityEngine;

public class UINumberCounter : MonoBehaviour
{
    public TMP_Text numberText;
    public float duration = 1.0f;

    private Coroutine countCoroutine;

    public void StartCount(int from, int to)
    {
        if (countCoroutine != null)
            StopCoroutine(countCoroutine);

        countCoroutine = StartCoroutine(CountNumber(from, to));
    }

    private IEnumerator CountNumber(int from, int to)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            numberText.text = current.ToString();
            yield return null;
        }

        numberText.text = to.ToString(); // Ensure final value is exact
    }
}
