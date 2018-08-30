using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public Animator animator;
    public FPMovementController _character;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (_character.IsRunning)
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running",false);
        }
    }
}
