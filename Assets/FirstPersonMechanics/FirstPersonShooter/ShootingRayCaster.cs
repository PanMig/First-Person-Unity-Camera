using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRayCaster : CameraRaycaster{

    public delegate void FireShot();
    public event FireShot onShotFired;

    public delegate void FireHit();
    public event FireHit onShotHit;

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            if (onShotFired != null) onShotFired();
            if (ShootRay())
            {
                onShotHit();
            } 
        }
	}

    public override bool ShootRay()
    {
        //the viewport vector is in (x,y) with 0 being bottom and 1 top of the viewport.
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out currentHit, rayRange)){
            return true;
        }
        return false;
    }
}
