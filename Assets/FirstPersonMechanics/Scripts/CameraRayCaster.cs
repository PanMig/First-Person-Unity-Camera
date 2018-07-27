using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayCaster : MonoBehaviour {

    Camera cam;
    RaycastHit currentHit;
    RaycastHit prevHit;
    public RaycastHit Hit
    {
        get { return currentHit; }
    }
    Ray cameraAim;
    [SerializeField] private float rayDist;

    public delegate void TargetChanged();
    public event TargetChanged onTargetChange;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        prevHit = currentHit;
    }
	
	// Update is called once per frame
	void Update () {
        if (RayHasHit())
        {
            // for entering check weather ray hits another object, so to reduce calls.
            if (prevHit.point != currentHit.point)
            {
                onTargetChange();
            }
            prevHit = currentHit;
        }
        else
        {
            // Case where hit fails to hit object
            Cursor.visible = false;
        }
    }

    // we want a nullable return type
    public bool RayHasHit()
    {
        // the viewport vector is in (x,y) with 0 being bottom and 1 top of the viewport.
        cameraAim = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(cameraAim, out currentHit, rayDist))
        {
            return true;
        }
        return false;
    }
}
