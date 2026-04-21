using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class DebugPanel : MonoBehaviour
{
    [Header("References:")]
    public GameObject displayParent;
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI msText;
    public TextMeshProUGUI memoryText;

    [Header("Settings:")]
    public float pollingTime = 0.5f;

    float timeAccumulator;
    int frameCount;

    public void ToggleDebugPanel()
    {
        if (displayParent != null)
        {
            bool newState = !displayParent.activeSelf;
            displayParent.SetActive(newState);
        }
    }

    void Update()
    {
        if (displayParent != null && !displayParent.activeInHierarchy)
            return;

        timeAccumulator += Time.deltaTime;
        frameCount++;

        if (timeAccumulator >= pollingTime)
        {
            UpdateDisplay();

            timeAccumulator -= pollingTime;
            frameCount = 0;
        }
    }

    void UpdateDisplay()
    {
        int fps = Mathf.RoundToInt(frameCount / timeAccumulator);
        if (fpsText != null) fpsText.text = $"FPS: {fps} FPS";

        float ms = (timeAccumulator / frameCount) * 1000f;
        if (msText != null) msText.text = $"Frametime: {ms:F2} ms";

        long allocatedMem = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        if (memoryText != null) memoryText.text = $"Memory: {allocatedMem} MB";
    }
}
