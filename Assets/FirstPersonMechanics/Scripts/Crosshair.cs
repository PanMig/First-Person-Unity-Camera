using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {

    [SerializeField] private Sprite pickUp;
    [SerializeField] private Sprite note;
    [SerializeField] private Sprite crosshair;

    [SerializeField] private InteractionRayCaster _raycaster;

    // Use this for initialization
    void Start () {
        _raycaster = Camera.main.GetComponent<InteractionRayCaster>();

        _raycaster.onTargetChange += ChangeCrosshair;
        _raycaster.onNoTarget += ChangeCrosshair;
    }

    private void OnDisable()
    {
        _raycaster.onTargetChange -= ChangeCrosshair;
        _raycaster.onNoTarget -= ChangeCrosshair;
    }

    void ChangeCrosshair()
    {
        if(_raycaster.Hit.collider != null)
        {
            switch (_raycaster.Hit.collider.tag)
            {
                case "pickUp":
                    ChangeIcon(pickUp);
                    break;
                case "note":
                    ChangeIcon(note);
                    break;
                default:
                    ChangeIcon(crosshair);
                    break;
            }
        }
        else
        {
            ChangeIcon(crosshair);
            return;
        }
    }

    void ChangeIcon(Sprite icon)
    {
        Image img = gameObject.GetComponent<Image>();
        img.sprite = icon;
    }
}
