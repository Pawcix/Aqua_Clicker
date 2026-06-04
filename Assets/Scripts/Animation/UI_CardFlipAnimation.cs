using System.Collections;
using TMPro;
using UnityEngine;

public class UI_CardFlipAnimation : MonoBehaviour
{
    [SerializeField] RectTransform targetRect;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] float flipDuration = 0.8f;
    [SerializeField] RiskReward riskRewardVisuals;

    Coroutine flipCoroutine;

    public void StartFlipAnimation(string finalResult)
    {
        if (targetRect == null || resultText == null) return;

        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
        }

        flipCoroutine = StartCoroutine(AnimateFlipWithEaseOut(finalResult));
    }

    IEnumerator AnimateFlipWithEaseOut(string finalResult)
    {
        float time = 0;

        if (resultText != null)
        {
            resultText.text = "Magic\nButtons";
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Risk - Drum");
        }

        float totalRotation = 3240f;

        while (time < flipDuration)
        {
            time += Time.deltaTime;

            float t = time / flipDuration;
            float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);
            float currentAngle = easedT * totalRotation;

            targetRect.localRotation = Quaternion.Euler(0, currentAngle, 0);

            yield return null;
        }

        targetRect.localRotation = Quaternion.identity;

        yield return new WaitForSeconds(1f);

        if (resultText != null)
        {
            resultText.text = finalResult;
        }

        if (riskRewardVisuals != null)
        {
            riskRewardVisuals.StartBoosterDisplay();
        }

        Object.FindFirstObjectByType<System_RiskReward>().ApplyFinalRewardAndStartCooldown();
    }
}