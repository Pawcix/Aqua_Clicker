using UnityEngine;

public class CursorTrail : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float zOffset = 10f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Cursor.visible = true;
    }

    void Update()
    {
        if (mainCamera == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = zOffset;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        transform.position = mouseWorldPos;
    }
}