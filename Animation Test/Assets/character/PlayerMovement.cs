using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    
    public Animator animator;
    public float mH;
    public float mV;
    public float TopSpeed;
    public float CSpeed;
    private float speed;
    public float gravityForce;
    public bool IsOnGround;
    public GameObject CameraOrient;
    public float jumpHeight;

    private Transform cam;
    private Rigidbody rb;
    private Vector3 lookPos;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;
    private float h;
    private bool IsCrouch;
    private bool IsJumping;
    private float Jgravity = 16;
    
    

    void Start()
    {
        speed = TopSpeed;
        //Animator animator = GetComponent<Animator>();
        IsJumping = false;
        IsOnGround = false;
        gravityForce = gravityForce / -1;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }
    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical");


        groundCheck();
        JumpPressed();
        if(IsOnGround == true)
        {
            h = 0;
        }

        if (IsOnGround == false)
        {
            h = gravityForce;
        }


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
        Rotate();
        // Check for ground (Main for animations, but also for jump limits)


        Move(move);
        //JumpPressed();

        if (Input.GetButtonDown("Fire3"))
        {
            IsCrouch = !IsCrouch;
            if (IsCrouch == false)
            {
                speed = TopSpeed;
            }
            if (IsCrouch == true)
            {
                speed = CSpeed;
            }

        }
        Vector3 movement = new Vector3(mH, h , mV * 2f);
        movement = CameraOrient.transform.TransformDirection(movement);
        NewMethod(movement);

    }

    private void NewMethod(Vector3 movement)
    {
        
        rb.velocity = movement * speed;
    }

    // so far not working
    void JumpPressed()
    {
        if (Input.GetButtonDown("Jump") && IsOnGround == true)
        {
            IsJumping = true;
            Debug.Log("Jumped");
            IsOnGround = false;
            h = Mathf.Sqrt(2f * Jgravity * jumpHeight);
            


        }
    }

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
    void Rotate()
    {

        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray ,out _hit))
        {
            lookPos = _hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;

        transform.LookAt(transform.position + lookDir, Vector3.up);

    }

    void groundCheck()
    {
        RaycastHit _gHit;
        Ray _gRay = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(_gRay, out _gHit, 1.7f))
        {
            IsOnGround = true;
        }
        else
        {
            IsOnGround = false;
        }

        animator.SetBool("IsGrounded", IsOnGround);
        Debug.Log(IsOnGround);
        Debug.DrawRay(transform.position, -transform.up * 1.7f, Color.white);
    }

    
    
}
