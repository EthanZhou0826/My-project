using UnityEngine;

public class PlayerAimer : MonoBehaviour
{
    public Transform visualRoot;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        AimAtMouse();
    }

    private void AimAtMouse()
    {
        if (cam == null || visualRoot == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -cam.transform.position.z;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dir = mouseWorldPos - visualRoot.position;

        if (dir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            visualRoot.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public float GetVisualRotationZ()
    {
        if (visualRoot == null) return 0f;
        return visualRoot.eulerAngles.z;
    }

    public void SetVisualRotation(float z)
    {
        if (visualRoot == null) return;
        visualRoot.rotation = Quaternion.Euler(0f, 0f, z);
    }
}