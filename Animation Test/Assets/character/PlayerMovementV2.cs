using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    
    private float movementSpeed;

    [SerializeField]
    private float accelerationFactor = 100f;

    [SerializeField]
    private float decelerationFactor = 16f;

    [SerializeField]
    private float gravity = 16f;

    [SerializeField]
    private float jumpHeight = 3f;

    private float nextGroundLeave;

    /// <summary>
    /// The input that this player should move with.
    /// </summary>
    public Vector2 Input { get; set; }

    /// <summary>
    /// Should the player jump right now?
    /// </summary>
    public bool Jump { get; set; }

    /// <summary>
    /// Is this player moving according to the input?
    /// </summary>
    public bool IsMoving => Input.sqrMagnitude > 0.5f;

    /// <summary>
    /// Is the player currently grounded?
    /// </summary>
    public bool IsGrounded { get; private set; }

    public bool Gravity { get; set; } = true;

    /// <summary>
    /// The rigidbody attached to this player.
    /// </summary>
    public Rigidbody Rigidbody { get; private set; }

    public Animator animator;
    public float mH;
    public float mV;
    public float TopSpeed;
    public float CSpeed;
    public float Xoffset;
    public GameObject CrouchHitBox;
    public GameObject normalHitBox;
     
    public GameObject CameraOrient;
    

    private Transform cam;
    private Rigidbody rb;
    private Vector3 lookPos;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;
    
    private bool IsCrouch;
    
    


    private void Awake()
    {
        normalHitBox.SetActive(true);
        CrouchHitBox.SetActive(false);
        movementSpeed = TopSpeed;
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        Rigidbody.freezeRotation = true;
        cam = Camera.main.transform;
    }
    
    void FixedUpdate()
    {

        //get velocity, lerp it to destination velocity, then assign it back
        Vector3 velocity = Rigidbody.velocity;
        Vector3 desiredVelocity = new Vector3(Input.x, 0f, Input.y).normalized * movementSpeed;
        Vector3 CorrectedDV = new Vector3(desiredVelocity.x , 0f, desiredVelocity.z * Xoffset);
        CorrectedDV.y = velocity.y;


        float acceleration = IsMoving ? accelerationFactor : decelerationFactor;
        velocity = Vector3.Lerp(velocity, CorrectedDV, Time.fixedDeltaTime * acceleration);
        velocity = CameraOrient.transform.TransformDirection(velocity);
        Rigidbody.velocity = velocity;

        if (Gravity)
        {
            ApplyGravity();
        }

        //leave ground after this timer
        if (Time.fixedTime > nextGroundLeave)
        {
            IsGrounded = false;
        }


        mH = Input.x;
        mV = Input.y;

        // setting the moemevnts for animations to be reletive to the camera
        if (cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1).normalized);
            move = mV * camForward + mH * cam.right;
        }
        else
        {
            move = mV * transform.forward + mH * transform.right;
        }
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        // rotate towards the mouse
        //Rotate();
        // Check for ground (Main for animations, but also for jump limits)
        Move(move);
        //JumpPressed();
        if (UnityEngine.Input.GetButtonDown("Fire3"))
        {
            IsCrouch = !IsCrouch;
            if (IsCrouch == false)
            {
                normalHitBox.SetActive(true);
                CrouchHitBox.SetActive(false);
                movementSpeed = TopSpeed;
            }
            if (IsCrouch == true)
            {
                normalHitBox.SetActive(false);
                CrouchHitBox.SetActive(true);
                movementSpeed = CSpeed;
            }

        }

        


    }

    private void Update()
    {
        //do jump check
        if (Jump && IsGrounded)
        {
            PerformJump();
        }
        animator.SetBool("IsGrounded", IsGrounded);
    }



    // Animation content start

    void Move(Vector3 move)
    {
        if(move.magnitude > 1)
        {
            move.Normalize();
        }


        this.moveInput = move;

        ConverMoveInput();
        UpdateAnimator();

        
    }

    void ConverMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    
    void UpdateAnimator()
    {
        /* START LOCOMOTION */
        // Update the Animator with our values so that the blend tree updates
        animator.SetFloat("Forward", forwardAmount * 4);
        animator.SetFloat("Turn", turnAmount);
        animator.SetBool("IsCrouching", IsCrouch);
        /* END LOCOMOTION */

    }

    //Animation content end


    public void PerformJump()
    {
        IsGrounded = false;
        Vector3 velocity = Rigidbody.velocity;
        velocity.y = Mathf.Sqrt(2f * gravity * jumpHeight);
        Rigidbody.velocity = velocity;
    }

    private void ApplyGravity()
    {
        Rigidbody.AddForce(Vector3.down * gravity);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            float angle = Vector3.Angle(contact.normal, Vector3.up);

            //tis the slope angle
            if (angle <= 45f)
            {
                IsGrounded = true;
                nextGroundLeave = Time.fixedTime + 0.15f;
            }
        }
    }
    void Rotate()
    {

        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
        
        if (Physics.Raycast(_ray ,out _hit))
        {
            lookPos = _hit.point;
        }
        

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);

    }

    

    
    
}
