using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;

public class Rewarded : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] System_Data data; 
    [SerializeField] Button _showAdButton;
    [SerializeField] TextMeshProUGUI _cooldownText;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId;

    private float _cooldownTimer = 0f;
    private const float MAX_COOLDOWN = 300f;

    void Awake()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdUnitId : _androidAdUnitId;
        _showAdButton.interactable = false;

        if (data.rewardedAdCooldownTimer > 0)
        {
            if (_cooldownText != null) _cooldownText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_cooldownTimer / 60F);
            int seconds = Mathf.FloorToInt(_cooldownTimer % 60F);
            _cooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (_cooldownTimer <= 0)
            {
                _cooldownText.gameObject.SetActive(false);
                LoadAd();
            }
        }
    }

    public void LoadAd()
    {
        if (_cooldownTimer <= 0)
            Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
#if UNITY_EDITOR
        Debug.Log("Symulacja reklamy w Edytorze - Nagroda przyznana!");
        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#else
        _showAdButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
#endif
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId)) _showAdButton.interactable = true;
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (System_Economy.Instance != null)
            {
                double current = System_Economy.Instance.GetPoints();
                System_Economy.Instance.AddPoints(current);
            }
            else
            {
                Debug.LogError("System_Economy.Instance nie istnieje! SprawdŸ czy obiekt jest w scenie.");
            }

            data.rewardedAdCooldownTimer = MAX_COOLDOWN;
            _showAdButton.interactable = false;
            if (_cooldownText != null) _cooldownText.gameObject.SetActive(true);
        }
    }

    void ResetMultiplier()
    {
        Clicker_System clicker = Object.FindAnyObjectByType<Clicker_System>();
        if (clicker != null)
        {
            System_Data data = clicker.GetComponent<System_Data>();
            if (data != null)
            {
                data.adMultiplier = 1.0f;
            }
        }
    }

    public void OnUnityAdsFailedToLoad(string id, UnityAdsLoadError e, string m) { }
    public void OnUnityAdsShowFailure(string id, UnityAdsShowError e, string m) { }
    public void OnUnityAdsShowStart(string id) { }
    public void OnUnityAdsShowClick(string id) { }
}