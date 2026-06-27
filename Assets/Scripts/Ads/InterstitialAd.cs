using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    private float _lastAdTime = -300f;
    private const float COOLDOWN_TIME = 300f;
    private const float START_DELAY = 180f;
    private float _startTime;

    void Awake()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdUnitId : _androidAdUnitId;
        _startTime = Time.time;
        _lastAdTime = _startTime - (COOLDOWN_TIME - START_DELAY);
    }

    void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        if (Time.time - _lastAdTime >= COOLDOWN_TIME)
        {
            Debug.Log("Pokazywanie reklamy...");
            Advertisement.Show(_adUnitId, this);
            _lastAdTime = Time.time;
        }
        else
        {
            Debug.Log("Za wczeœnie na reklamê (cooldown).");
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && (Time.time - _startTime >= START_DELAY))
        {
            ShowAd();
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId) { }
    public void OnUnityAdsFailedToLoad(string id, UnityAdsLoadError e, string m) { }
    public void OnUnityAdsShowFailure(string id, UnityAdsShowError e, string m) { }
    public void OnUnityAdsShowStart(string id) { }
    public void OnUnityAdsShowClick(string id) { }

    public void OnUnityAdsShowComplete(string id, UnityAdsShowCompletionState state)
    {
        LoadAd();
    }
}