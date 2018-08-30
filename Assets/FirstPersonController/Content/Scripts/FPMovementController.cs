using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPMovementController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool jumpingEnabled = true;
    [SerializeField] private float jumpSpeed;
    [Range(0.0f, 0.5f)]
    [SerializeField] private float fallRate;
    [SerializeField] private bool  slopLimitEnabled;
    [SerializeField] private float slopeLimit;
    [Header("Sounds")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip landingClip;
    [SerializeField] private AudioClip jumpingClip;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float footstepsVol;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float footstepspitch;
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool canJump = true;
    private bool prevGrounded = false;
    private bool grounded;

    private Rigidbody rb;
    private CapsuleCollider _capsule;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDirection;
    private float distanceToPoints;
    private CameraHeadBob headBob;
    private Vector3 YAxisGravity;
    private AudioSource audioSource;

    #region Properties
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }

        set
        {
            isRunning = value;
        }
    }

    public bool IsLanding
    {
        get
        {
            return isLanding;
        }

        set
        {
            isLanding = value;
        }
    }
    #endregion


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();
        headBob = transform.GetComponentInChildren<CameraHeadBob>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && jumpingEnabled && canJump)
        {
            isJumping = true;
            Jump();
            PlayJumpingSound();
        }

        horizontalMovement = 0; verticalMovement = 0;

        //Calculate FPcontroller movement direction through WASD and arrows Input
        if (AllowMovement(transform.right * Input.GetAxis("Horizontal")))
        {
            horizontalMovement = Input.GetAxis("Horizontal");
        }
        if (AllowMovement(transform.forward * Input.GetAxis("Vertical")))
        {
            verticalMovement = Input.GetAxis("Vertical");
        }
        //normalize vector so movement in two axis simultanesly is balanced.
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

        //Toggle run & jump
        IsRunning = Input.GetKey(KeyCode.LeftShift);

        if (!prevGrounded && IsGrounded())
        {
            StartCoroutine(headBob.LandingBob());
            PlayLandingSound();
        }

        prevGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        /* When calculating the moveDirection , the Y velocity always stays 0. 
         As a result the player is falling very slowy. 
         To solve this we add to the rb velocity the Y axis velocity */

        YAxisGravity = new Vector3(0, rb.velocity.y - fallRate, 0);
        if (!isJumping) { Move(); }
        rb.velocity += YAxisGravity;
    }

    #region Player Movement

    public void Move()
    {
        if (!IsRunning)
        {
            rb.velocity = moveDirection * walkSpeed * Time.fixedDeltaTime * 100;
        }
        else
        {
            rb.velocity = moveDirection * runSpeed * Time.fixedDeltaTime * 100;
        }

        PlayFootStepsSound();
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
        }
    }

    #endregion

    #region Sounds
    public void PlayFootStepsSound()
    {
        if (IsGrounded() && rb.velocity.magnitude > 5.0f && !audioSource.isPlaying)
        {
            if (IsRunning)
            {
                audioSource.volume = footstepsVol;
                //audioSource.pitch = Random.Range(footstepspitch, footstepspitch + 0.15f);
                audioSource.pitch = footstepspitch;
            }
            else
            {
                audioSource.volume = footstepsVol / 2;
                audioSource.pitch = .8f;
            }

            audioSource.PlayOneShot(footstepClip);
        }
    }

    public void PlayLandingSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(landingClip);
        }
    }

    public void PlayJumpingSound()
    {
        audioSource.PlayOneShot(jumpingClip);
    }
    #endregion

    #region RayCasting

    // make a capsule cast to check weather there is an obstavle in front of the player ONLY when jumping.
    public bool AllowMovement(Vector3 castDirection)
    {
        Vector3 point1;
        Vector3 point2;
        if (!IsGrounded())
        {
            // The distance from the bottom and top of the capsule
            distanceToPoints = _capsule.height / 2 - _capsule.radius;
            /*Top and bottom capsule points respectively, transform.position is used to get points relative to 
               local space of the capsule. */
            point1 = transform.position + _capsule.center + Vector3.up * distanceToPoints;
            point2 = transform.position + _capsule.center + Vector3.down * distanceToPoints;
            float radius = _capsule.radius * .95f;
            float capsuleCastDist = 0.1f;

            if (Physics.CapsuleCast(point1, point2, radius, castDirection, capsuleCastDist))
            {
                return false;
            }
        }
        if (slopLimitEnabled && IsGrounded())
        {
            float castDist = _capsule.height;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + _capsule.center, Vector3.down, out hit, castDist)
                && IsGrounded())
            {
                float currentsSlope = Vector3.Angle(hit.normal, transform.forward) - 90.0f;
                if (currentsSlope > slopeLimit)
                {
                    canJump = false;
                    return false;
                }
            }
        }
        canJump = true;
        return true;
    }

    // Make a sphere cast with down direction to define weather the player is touching the ground.
    public bool IsGrounded()
    {
        Vector3 capsule_bottom = transform.position + _capsule.center + Vector3.down * distanceToPoints;
        float radius = 0.1f;
        float maxDist = 1.0f;
        RaycastHit hitInfo;
        if (Physics.SphereCast(capsule_bottom, radius, Vector3.down, out hitInfo, maxDist))
        {
            isJumping = false;
            return true;
        }
        return false;
    }

    #endregion
}
