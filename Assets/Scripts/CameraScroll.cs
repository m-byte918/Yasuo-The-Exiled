using UnityEngine;

public class CameraScroll : MonoBehaviour {
    public Camera cam;
    public float zoomSpeed;

    private float camFOV;
    private float mouseScrollInput;

    void Start() {
        camFOV = cam.fieldOfView;
    }

    void Update() {
        mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        camFOV -= mouseScrollInput * zoomSpeed;
        camFOV = Mathf.Clamp(camFOV, 30, 60);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFOV, zoomSpeed);
    }
}
