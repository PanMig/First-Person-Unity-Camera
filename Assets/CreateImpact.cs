using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateImpact : MonoBehaviour {

    private ShootingRayCaster _raycaster;
    public GameObject impactFX;


    // Use this for initialization
    void Start () {
        _raycaster = Camera.main.GetComponent<ShootingRayCaster>();
        _raycaster.onShotHit += CreateImpact;
    }

    void CreateImpact()
    {
        Instantiate(impactFX,_raycaster.Hit.point, Quaternion.LookRotation(_raycaster.Hit.normal));
    }
}
