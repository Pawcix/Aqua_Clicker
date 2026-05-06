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
        }
    }
}