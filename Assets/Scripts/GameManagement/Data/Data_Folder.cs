using UnityEngine;

public class Data_Folder : MonoBehaviour
{
    [ContextMenu("Open Save Folder")]
    public void OpenFolder()
    {
        string path = Application.persistentDataPath;

        if (System.IO.Directory.Exists(path))
        {
            Application.OpenURL(path);
            // Debug.Log($"<color=green><b>[Folder Opener]</b></color> Opening folder: {path}");
        }
        else
        {
            // Debug.LogError("[Folder Opener] Path does not exist!");
        }
    }
}