using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] TextMeshProUGUI counterPointsPerSecond;
    [SerializeField] Button clickerButton;

    private void Start()
    {
        StartCoroutine(BalanceAnimation());
        StartCoroutine(PulseAnimationClickerButton());
    }

    IEnumerator BasePulseAnimation(RectTransform rectTransform, float duration, float maxScale, float minScale, bool loop = false)
    {
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        do
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float scale = Mathf.Lerp(minScale, maxScale, t / duration);
                rectTransform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float scale = Mathf.Lerp(maxScale, minScale, t / duration);
                rectTransform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }
        while (loop);

        rectTransform.localScale = new Vector3(minScale, minScale, minScale);
    }

    public IEnumerator PulseAnimation()
    {
        yield return BasePulseAnimation(counterText.rectTransform, 0.1f, 1.05f, 1f);
    }

    public IEnumerator PulseAnimationClickerButton()
    {
        yield return BasePulseAnimation(clickerButton.GetComponent<RectTransform>(), 1f, 1.05f, 1f, true);
    }

    public IEnumerator BalanceAnimation()
    {
        RectTransform rectTransform;
        rectTransform = counterPointsPerSecond.GetComponent<RectTransform>();

        float swingAmount = 2.5f;
        float swingDuration = 2.5f;

        float halfDuration = swingDuration / 2f;
        float elapsedTime = 0f;

        while (true)
        {
            while (elapsedTime < halfDuration)
            {
                float t = elapsedTime / halfDuration;
                float angle = Mathf.Sin(t * Mathf.PI) * -swingAmount;
                rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < halfDuration)
            {
                float t = elapsedTime / halfDuration;
                float angle = Mathf.Sin(t * Mathf.PI) * swingAmount;
                rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
        }
    }
}
