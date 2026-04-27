using UnityEngine;
using System.Collections.Generic;

public class System_Data : MonoBehaviour
{
    [Header("Economy:")]
    public int pointsCounter = 0;
    public int pointsPerClick = 1;
    public int pointsPerSecond = 0;
    public int totalAwayEarnings = 0;

    [Header("Workers Progress:")]
    public List<int> workerLevels = new List<int>();

    [Header("Time:")]
    public float timer = 0f;

    [Header("Skills Data:")]
    public int currentSkinIndex = 0;
    public bool isAutoClickerActive = false;
}
