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

    bool isLuckyBonusFlashing = false;
    Color currentNumberColor;
    Color originalColor;
    Color bonusColor;

    public static PointsDisplay Instance;

    void Awake()
    {
        Instance = this;

        if (pointsText != null)
        {
            originalColor = pointsText.color;
            currentNumberColor = originalColor;
        }

        if (ColorUtility.TryParseHtmlString("#E84E40", out Color parsedColor))
        {
            bonusColor = parsedColor;
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

        if (System.Math.Abs(currentFloorValue - lastDisplayedValue) > 0.9 || isLuckyBonusFlashing)
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

            if (isLuckyBonusFlashing)
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

    public void PulseLuckyBonus()
    {
        transform.localScale = Vector3.one * 1.2f;

        if (pointsText != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(FlashColorRoutine());
        }
    }

    IEnumerator FlashColorRoutine()
    {
        isLuckyBonusFlashing = true;
        currentNumberColor = bonusColor;

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;

            currentNumberColor = Color.Lerp(bonusColor, originalColor, t);
            yield return null;
        }

        currentNumberColor = originalColor;
        isLuckyBonusFlashing = false;
        UpdateText(lastDisplayedValue);
    }
}