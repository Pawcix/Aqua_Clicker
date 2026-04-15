using UnityEngine;

public class Modal_MusicSounds : MonoBehaviour
{
    public GameObject musicAndSoundsModal;

    void Awake()
    {
        if (musicAndSoundsModal != null)
        {
            musicAndSoundsModal.SetActive(false);
        }
    }

    public void ToggleExit()
    {
        if (musicAndSoundsModal != null)
        {
            bool newState = !musicAndSoundsModal.activeSelf;
            musicAndSoundsModal.SetActive(newState);
        }
    }
}
