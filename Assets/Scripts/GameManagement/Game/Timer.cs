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

    public void LoadSavedTime(float time)
    {
        TotalPlayTime = time;
    }

    public string GetFormattedTime()
    {
        int t = Mathf.FloorToInt(TotalPlayTime);
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;

        if (h > 0) return $"{h:00}:{m:00}:{s:00}";
        if (m > 0) return $"{m:00}:{s:00}";
        return $"{s}s";
    }

}
