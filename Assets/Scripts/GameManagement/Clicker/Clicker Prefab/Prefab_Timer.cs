using TMPro;
using UnityEngine;

public class Prefab_Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    float time;

    void Update()
    {
        time += Time.deltaTime;
        FormatAndDisplayText();
    }

    public void UpdateTimerPrefab()
    {
        FormatAndDisplayText();
    }

    void FormatAndDisplayText()
    {
        if (timerText == null) return;

        int t = Mathf.RoundToInt(time);
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;

        if (h > 0)
            timerText.text = $"Time \n{h:D1}h {m:D1}m {s:D1}s";
        else if (m > 0)
            timerText.text = $"Time \n{m:D1}m {s:D1}s";
        else
            timerText.text = $"Time \n{s:D1}s";
    }
}
