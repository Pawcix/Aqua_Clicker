using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ClickCounter_CPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cpsText;

    private List<float> clickTimestamps = new List<float>();
    private float lastClickTime = 0f;
    private const float timeWindow = 1.0f;
    private const float hideTimeout = 3.0f;

    void Update()
    {
        float currentTime = Time.time;

        clickTimestamps.RemoveAll(timestamp => currentTime - timestamp > timeWindow);

        if (currentTime - lastClickTime > hideTimeout)
        {
            if (cpsText != null) cpsText.text = "";
        }
        else
        {
            if (cpsText != null)
            {
                cpsText.text = $"CPS: {clickTimestamps.Count}/s";
            }
        }
    }

    public void RegisterClick()
    {
        clickTimestamps.Add(Time.time);
        lastClickTime = Time.time;
    }
}