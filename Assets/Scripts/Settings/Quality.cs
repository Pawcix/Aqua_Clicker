using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Quality : MonoBehaviour
{
    [SerializeField] TMP_Dropdown qualityDropdown;

    void Start()
    {
        qualityDropdown.ClearOptions();

        List<string> options = new List<string>(QualitySettings.names);

        qualityDropdown.AddOptions(options);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void SetQuality(int index)
    {
        // Debug.Log("Quality level: " + QualitySettings.names[index]);
    }
}
