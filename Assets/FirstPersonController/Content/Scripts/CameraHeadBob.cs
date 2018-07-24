using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHeadBob : MonoBehaviour
{
    private float timer = 0.0f;
    [Header("MovementBob")]
    [SerializeField] private float bobbingSpeed;
    [SerializeField] private float bobbingAmount;
    [SerializeField] private float bobbingRunSpeed;
    [SerializeField] private float bobbingRunAmount;
    [SerializeField] private float restPosition;// the position where the camera sits when not bobbing.
    [Header("JumpBob")]
    [SerializeField] private float jumpBobDuration;
    [SerializeField] private float jumpBobAmount;
    private float offset;
    float horizontal;
    float vertical;
    float jump;
    Vector3 localPos;
    private FPMovementController _character;


    private void Awake()
    {
        _character = transform.parent.GetComponent<FPMovementController>();
    }

    void Update()
    {
        float waveslice = 0.0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jump = Input.GetAxis("Jump");

        localPos = transform.localPosition;
   
        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            if (!_character.IsRunning)
            {
                timer = timer + bobbingSpeed;
            }
            else
            {
                timer = timer + bobbingRunSpeed;
            }
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange;
            if (!_character.IsRunning)
            {
                translateChange = waveslice * bobbingAmount;
            }
            else
            {
                translateChange = waveslice * bobbingRunAmount;
            }
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            localPos.y = restPosition + translateChange;
        }
        else
        {
            localPos.y = restPosition;
            localPos.y = restPosition + offset;
        }

        transform.localPosition = localPos;
    }

    public IEnumerator LandingBob()
    {
        // make the camera move down slightly
        float t = 0f;
        while (t < jumpBobDuration)
        {
            offset = Mathf.Lerp(0f, -jumpBobAmount, t / jumpBobDuration);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // make it move back to neutral
        t = 0f;
        while (t < jumpBobDuration)
        {
            offset = Mathf.Lerp(-jumpBobAmount, 0f, t / jumpBobDuration);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        offset = 0f;
    }
}