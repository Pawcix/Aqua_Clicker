using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId = "1234567";
    [SerializeField] private string iosGameId = "7654321";
    [SerializeField] private bool testMode;
    private string gameId;

    private void Awake()
    {
#if UNITY_IOS
                gameId = iosGameId;
#elif UNITY_ANDROID
                gameId = androidGameId;
#elif UNITY_EDITOR
                gameId= androidGameId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Unity Ads initialization failed: " + message);
    }
}
