using TMPro;
using UnityEngine;
using System.Collections;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI pointsText;

    [Header("Settings:")]
    [SerializeField] string prefix = "WATERS\n";
    [SerializeField] float speedMultiplier = 15f;
    [SerializeField] float pulseSpeed = 10f;

    [Header("Color Flash Settings:")]
    [SerializeField] float flashDuration = 0.6f;

    double lastDisplayedValue = -1.0;
    double displayedPoints;

    Coroutine colorCoroutine;

    bool isColorFlashing = false;
    Color currentNumberColor;
    Color originalColor;
    Color bonusColor;
    Color comboColor;
    Color critComboColor;
    Color goldenDropColor; 
    Color targetFlashColor;

    public static PointsDisplay Instance;

    void Awake()
    {
        Instance = this;

        if (pointsText != null)
        {
            originalColor = pointsText.color;
            currentNumberColor = originalColor;
        }

        if (ColorUtility.TryParseHtmlString("#E84E40", out Color parsedBonusColor))
        {
            bonusColor = parsedBonusColor;
        }

        if (ColorUtility.TryParseHtmlString("#FFBF00", out Color parsedComboColor))
        {
            comboColor = parsedComboColor;
        }

        if (ColorUtility.TryParseHtmlString("#E800FF", out Color parsedCritComboColor))
        {
            critComboColor = parsedCritComboColor;
        }

        if (ColorUtility.TryParseHtmlString("#FFCD00", out Color parsedGoldenColor))
        {
            goldenDropColor = parsedGoldenColor;
        }
    }

    void Start()
    {
        if (data != null)
            displayedPoints = data.pointsCounterFloat;

        UpdateText(System.Math.Floor(displayedPoints));
    }

    void Update()
    {
        if (pointsText == null || data == null) return;

        double target = data.pointsCounterFloat;

        if (speedMultiplier <= 0.01f)
        {
            displayedPoints = target;
        }
        else
        {
            double distance = System.Math.Abs(target - displayedPoints);
            double step = (distance + (double)data.pointsPerSecond + 1.0) * Time.deltaTime * speedMultiplier;
            displayedPoints = MoveTowardsDouble(displayedPoints, target, step);
        }

        double currentFloorValue = System.Math.Floor(displayedPoints);

        if (System.Math.Abs(currentFloorValue - lastDisplayedValue) > 0.9 || isColorFlashing)
        {
            lastDisplayedValue = currentFloorValue;
            UpdateText(lastDisplayedValue);
        }

        if (transform.localScale.x > 1.0f)
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * pulseSpeed);
    }

    double MoveTowardsDouble(double current, double target, double maxDelta)
    {
        if (System.Math.Abs(target - current) <= maxDelta)
        {
            return target;
        }
        return current + System.Math.Sign(target - current) * maxDelta;
    }

    void UpdateText(double value)
    {
        if (pointsText == null) return;

        if (value < 0.1)
        {
            pointsText.text = prefix.TrimEnd();
        }
        else
        {
            string formattedValue = NumberFormatter.FormatWithDots(value);

            if (isColorFlashing)
            {
                string hexColor = ColorUtility.ToHtmlStringRGBA(currentNumberColor);
                pointsText.text = $"{prefix}<color=#{hexColor}>{formattedValue}</color>";
            }
            else
            {
                pointsText.text = prefix + formattedValue;
            }
        }
    }

    public void Pulse()
    {
        transform.localScale = Vector3.one * 1.15f;
    }

    public void PulseGoldenDrop()
    {
        transform.localScale = Vector3.one * 1.25f;

        if (pointsText != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(FlashColorRoutine(goldenDropColor));
        }
    }

    public void PulseLuckyBonus()
    {
        transform.localScale = Vector3.one * 1.2f;

        if (pointsText != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(FlashColorRoutine(bonusColor));
        }
    }

    public void PulseComboEnd()
    {
        transform.localScale = Vector3.one * 1.25f;

        if (pointsText != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(FlashColorRoutine(comboColor));
        }
    }

    public void PulseCritComboEnd()
    {
        transform.localScale = Vector3.one * 1.35f;

        if (pointsText != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(FlashColorRoutine(critComboColor)); 
        }
    }

    IEnumerator FlashColorRoutine(Color flashColor)
    {
        isColorFlashing = true;
        targetFlashColor = flashColor;
        currentNumberColor = targetFlashColor;

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;

            currentNumberColor = Color.Lerp(targetFlashColor, originalColor, t);
            yield return null;
        }

        currentNumberColor = originalColor;
        isColorFlashing = false;
        UpdateText(lastDisplayedValue);
    }
}