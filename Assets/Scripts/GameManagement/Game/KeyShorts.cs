using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class KeyShorts : MonoBehaviour
{
    public KeyCode toggleKey;
    public UnityEvent onKeyPressed;
    public List<GameObject> allModals = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            onKeyPressed.Invoke();
        }
    }

    public void CloseAllModals()
    {
        foreach (GameObject modal in allModals)
        {
            if (modal != null && modal.activeInHierarchy)
            {
                UI_JuicyModal juicyAnim = modal.GetComponent<UI_JuicyModal>();

                if (juicyAnim != null)
                {
                    juicyAnim.CloseModal();
                }
                else
                {
                    modal.SetActive(false);
                }
            }
        }
    }
}