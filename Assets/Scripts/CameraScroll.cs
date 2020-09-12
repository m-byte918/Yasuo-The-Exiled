using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed;

    private float camFOV;
    private float mouseScrollInput;

    // Start is called before the first frame update
    void Start()
    {
        camFOV = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        camFOV -= mouseScrollInput * zoomSpeed;
        camFOV = Mathf.Clamp(camFOV, 30, 60);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camFOV, zoomSpeed);
    }
}
