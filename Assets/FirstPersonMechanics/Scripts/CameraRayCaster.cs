using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraRaycaster : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    protected RaycastHit currentHit;
    protected RaycastHit prevHit;
    protected Vector3 camAim;
    [SerializeField] protected float rayRange;

    public RaycastHit Hit
    {
        get { return currentHit; }
    }

    public abstract bool ShootRay();
}