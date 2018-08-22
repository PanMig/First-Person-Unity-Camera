using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionRayCaster))]
public class CursorController : MonoBehaviour {

    [SerializeField] private Texture2D pickUp;
    [SerializeField] private Texture2D note;

    InteractionRayCaster _raycaster;

    // Use this for initialization
    void Start () {
        _raycaster = GetComponent<InteractionRayCaster>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _raycaster.onTargetChange += ChangeCursor;
        _raycaster.onNoTarget += HideCursor;
    }

    private void OnDisable()
    {
        _raycaster.onTargetChange -= ChangeCursor;
        _raycaster.onNoTarget -= HideCursor;
    }

    // Update is called once per frame
    void Update () {
        //Unlock Cursor
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ChangeCursor()
    {
        if (_raycaster.Hit.collider.tag == "pickUp")
        {
            ChangeCursorIcon(pickUp);
        }
        else if (_raycaster.Hit.collider.tag == "note")
        {
            ChangeCursorIcon(note);
        }
        else
        {
            Cursor.visible = false;
            return;
        }
    }

    void HideCursor()
    {
        Cursor.visible = false;
    }

    public void ChangeCursorIcon(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.ForceSoftware);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
