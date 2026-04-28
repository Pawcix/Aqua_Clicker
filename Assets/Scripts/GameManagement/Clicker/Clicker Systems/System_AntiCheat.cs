using UnityEngine;

public class System_AntiCheat : MonoBehaviour
{
    [Header("Anticheat Settings:")]
    [SerializeField] int maxAllowedCPS = 15;
    [SerializeField] float interval = 1f;

    [Header("References:")]
    [SerializeField] Modal_AntiCheat antiCheatModal;

    int clickCount = 0;
    float timer = 0f;
    bool isModalOpen = false;

    public bool CheckClickLegal()
    {
        if (isModalOpen) return false;

        if (Object.FindFirstObjectByType<System_Data>().isAntiCheatBypassActive)
        {
            return true;
        }

        clickCount++;
        if (clickCount > maxAllowedCPS)
        {
            TriggerCheatWarning();
            return false;
        }
        return true;
    }

    void TriggerCheatWarning()
    {
        if (antiCheatModal != null)
        {
            isModalOpen = true;
            antiCheatModal.ShowModal();
        }
    }

    public void ResetModalState()
    {
        isModalOpen = false;
        clickCount = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            clickCount = 0;
            timer = 0f;
        }
    }
}
