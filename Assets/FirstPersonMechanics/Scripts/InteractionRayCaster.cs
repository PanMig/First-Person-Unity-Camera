using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRayCaster : CameraRaycaster {

    public delegate void TargetChanged();
    public event TargetChanged onTargetChange;

    public delegate void NoTarget();
    public event NoTarget onNoTarget;

    // Use this for initialization
    void Start () {
        prevHit = currentHit;
    }
	
	// Update is called once per frame
	void Update () {
        // normal every frame cast for object interaction
        if (ShootRay())
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
            if(onNoTarget != null) onNoTarget();
        }
    }

    // we want a nullable return type
    public override bool ShootRay()
    {
        // the viewport vector is in (x,y) with 0 being bottom and 1 top of the viewport.
        camAim = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(camAim,cam.transform.forward ,out currentHit, rayRange))
        {
            return true;
        }
        return false;
    }
}
