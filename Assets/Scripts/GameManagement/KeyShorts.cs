using UnityEngine;
using UnityEngine.Events;

public class KeyShorts : MonoBehaviour
{
    public KeyCode toggleKey;
    public UnityEvent onKeyPressed;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Debug.Log($"[KeyShorts] Wykryto klawisz {toggleKey}. Przełączam stan...");
            onKeyPressed.Invoke();
        }
    }
}
