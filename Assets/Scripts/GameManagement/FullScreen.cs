using UnityEngine;

public class FullScreen : MonoBehaviour
{
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Full Screen: " + isFullscreen);
    }
}
