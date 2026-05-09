using UnityEngine;

public class System_Critical : MonoBehaviour
{
    public static System_Critical Instance;

    [SerializeField] System_Data data;
    [SerializeField] System_CriticalEffect critEffect;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public double CalculateCriticalDamage(double basePoints, out bool wasCritical)
    {
        wasCritical = false;

        if (Random.value <= data.critChance)
        {
            wasCritical = true;

            if (critEffect != null)
                critEffect.ShowCritEffect();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("Crit_Sound");

            return basePoints * data.critMultiplier;
        }

        return basePoints;
    }
}
