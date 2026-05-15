using UnityEngine;

public class CursorEffect : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] GameObject cursorTrailObject;

    const string TrailPreferenceKey = "WaterTrailEnabled";

    void Start()
    {
        bool isTrailEnabled = PlayerPrefs.GetInt(TrailPreferenceKey, 1) == 1;

        if (cursorTrailObject != null)
        {
            cursorTrailObject.SetActive(isTrailEnabled);
        }
    }

    public void SetWaterTrail(bool isEnabled)
    {
        if (cursorTrailObject != null)
        {
            cursorTrailObject.SetActive(isEnabled);
        }

        PlayerPrefs.SetInt(TrailPreferenceKey, isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}
