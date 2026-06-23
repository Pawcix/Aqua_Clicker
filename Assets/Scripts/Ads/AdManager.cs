using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;
    public AdsInitializer initializer;
    public Rewarded rewardedScript;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Invoke(nameof(StartLoading), 1.0f);
    }

    void StartLoading()
    {
        if (AdsInitializer.isInitialized)
        {
            rewardedScript.LoadAd();
        }
    }
}