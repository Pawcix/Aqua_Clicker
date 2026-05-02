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

    double lastDisplayedValue = -1.0;
    double displayedPoints;

    public static PointsDisplay Instance;

    void Awake()
    {
        Instance = this;
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

        if (System.Math.Abs(currentFloorValue - lastDisplayedValue) > 0.9)
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
        if (value < 0.1)
        {
            pointsText.text = prefix.TrimEnd();
        }
        else
        {
            string formattedValue = NumberFormatter.FormatWithDots(value);
            pointsText.text = prefix + formattedValue;
        }
    }

    public void Pulse()
    {
        transform.localScale = Vector3.one * 1.15f;
    }
}
