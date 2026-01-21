using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BoardCameraController : MonoBehaviour
{
    [Header("Zoom")]
    public float zoomSpeed = 3f;
    public float minZoom = 3f;
    public float maxZoom = 30f;

    [Header("Pan")]
    public float panSpeed = 1f;

    [Header("Presets (1~5)")]
    public CameraPreset[] presets = new CameraPreset[5];

    private Camera cam;
    private Vector3 lastMouseWorld;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;

        // 프리셋 초기화 방지
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i] == null)
                presets[i] = new CameraPreset();
        }
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    // ===============================
    // Zoom (Wheel)
    // ===============================
    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) < 0.01f)
            return;

        Vector3 before = cam.ScreenToWorldPoint(Input.mousePosition);

        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        Vector3 after = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position += (before - after);
    }

    // ===============================
    // Pan (Middle Mouse)
    // ===============================
    void HandlePan()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 current = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += (lastMouseWorld - current) * panSpeed;
        }
    }

    // ===============================
    // Preset keys 1~5
    // ===============================

    public void ApplyPreset(int index)
    {
        if (index < 0 || index >= presets.Length)
            return;

        var p = presets[index];
        transform.position = new Vector3(p.position.x, p.position.y, transform.position.z);
        cam.orthographicSize = p.zoom;
    }

    // ===============================
    // 프리셋 저장용 (에디터 / 단축키)
    // ===============================
    public void SavePreset(int index)
    {
        if (index < 0 || index >= presets.Length)
            return;

        presets[index].position = transform.position;
        presets[index].zoom = cam.orthographicSize;
    }
}

[System.Serializable]
public class CameraPreset
{
    public Vector3 position;
    public float zoom;
}
