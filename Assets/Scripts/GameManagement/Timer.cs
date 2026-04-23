using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }
    public float TotalPlayTime { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        TotalPlayTime += Time.deltaTime;
    }

    public string GetFormattedTime()
    {
        int t = Mathf.RoundToInt(TotalPlayTime);
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;

        if (h > 0) return $"{h:D1}h {m:D1}m {s:D1}s";
        if (m > 0) return $"{m:D1}m {s:D1}s";
        return $"{s:D1}s";
    }
}
