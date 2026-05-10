using UnityEngine;

public class Mastery_Critical : MonoBehaviour
{
    public void OnCriticalHit()
    {
        Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Critical, 5f);
    }
}
