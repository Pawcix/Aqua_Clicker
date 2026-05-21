using UnityEngine;

public class UI_NotificationPulse : MonoBehaviour
{
    [Header("Pulse Settings:")]
    [SerializeField] float minScale = 1.0f;
    [SerializeField] float maxScale = 1.05f;
    [SerializeField] float pulseSpeed = 2.0f;

    Vector3 initialScale;
    float animationTimer = 0f;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        animationTimer = 0f;
    }

    void Update()
    {
        animationTimer += Time.unscaledDeltaTime * pulseSpeed;

        float rawProgress = Mathf.PingPong(animationTimer, 1f);
        float smoothProgress = Mathf.SmoothStep(0f, 1f, rawProgress);
        float currentScale = Mathf.Lerp(minScale, maxScale, smoothProgress);

        transform.localScale = initialScale * currentScale;
    }

    void OnDisable()
    {
        transform.localScale = initialScale;
    }
}