using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    public static bool isInitialized = false;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        string gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        isInitialized = true;
        Debug.Log("Unity Ads: Inicjalizacja zakończona sukcesem.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads: Błąd inicjalizacji: {message}");
    }
}