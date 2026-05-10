using UnityEngine;

public class Mastery_Clicks : MonoBehaviour
{
    public void OnPlayerClicked()
    {
        Mastery.Instance.AddMasteryXP(Mastery.MasteryType.Click, 1f);
    }
}
