using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] System_Data data;
    [SerializeField] TextMeshProUGUI pointsText;

    [Header("Settings:")]
    [SerializeField] string prefix = "WATERS\n";
    [SerializeField] float speedMultiplier = 15f;
    [SerializeField] float pulseSpeed = 10f;

    int lastPointInt = -1;
    float displayedPoints;

    void Start()
    {

        if (data != null)
            displayedPoints = data.pointsCounterFloat;

        UpdateText(Mathf.RoundToInt(displayedPoints));
    }

    void Update()
    {
        if (pointsText == null || data == null) return;

        float target = data.pointsCounterFloat;

        if (speedMultiplier <= 0.01f)
        {
            displayedPoints = target;
        }
        else
        {
            float distance = Mathf.Abs(target - displayedPoints);
            float step = (distance + data.pointsPerSecond + 1f) * Time.deltaTime * speedMultiplier;
            displayedPoints = Mathf.MoveTowards(displayedPoints, target, step);
        }

        int currentPointInt = Mathf.RoundToInt(displayedPoints);

        if (currentPointInt != lastPointInt)
        {
            lastPointInt = currentPointInt;
            UpdateText(lastPointInt);
        }

        if (transform.localScale.x > 1.0f)
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * pulseSpeed);
    }

    void UpdateText(int value)
    {
        string formattedValue = NumberFormatter.FormatWithDots(value);
        pointsText.text = prefix + formattedValue;
    }

    public void Pulse()
    {
        transform.localScale = Vector3.one * 1.15f;
    }
}
