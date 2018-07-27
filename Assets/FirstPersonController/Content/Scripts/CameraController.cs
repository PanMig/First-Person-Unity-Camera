using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    //camera public values
    public float XMinRotation;
    public float XMaxRotation;
    [Range(1.0f, 10.0f)]
    public float Xsensitivity;
    [Range(1.0f, 10.0f)]
    public float Ysensitivity;
    private Camera cam;
    private float rotAroundX, rotAroundY;
    private bool camMoved = false;

    // Use this for initialization
    void Start()
    {
        cam = this.GetComponent<Camera>();
        rotAroundX = transform.eulerAngles.x;
        rotAroundY = transform.eulerAngles.y;
    }

    private void Update()
    {
        rotAroundX += Input.GetAxis("Mouse Y") * Xsensitivity;
        rotAroundY += Input.GetAxis("Mouse X") * Ysensitivity;

        // Clamp rotation values
        rotAroundX = Mathf.Clamp(rotAroundX, XMinRotation, XMaxRotation);

        CameraRotation();
    }

    private void CameraRotation()
    {
        transform.parent.rotation = Quaternion.Euler(0, rotAroundY, 0); // rotation of parent (player body)
        cam.transform.rotation = Quaternion.Euler(-rotAroundX, rotAroundY, 0); // rotation of Camera
    }


}
