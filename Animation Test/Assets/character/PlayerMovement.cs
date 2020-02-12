using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    
    public Animator animator;
    public float mH;
    public float mV;
    public float speed;
    public float gravityForce;
    public bool IsOnGround;
    public GameObject CameraOrient;


    private Transform cam;
    private Rigidbody rb;
    private Vector3 lookPos;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;
    private float gravity;

    public float JumpHeight;
    private bool IsJumping;

    

    void Start()
    {

        //Animator animator = GetComponent<Animator>();
        IsJumping = false;
        IsOnGround = false;
        gravityForce = gravityForce / -1;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical") ;

        groundCheck();

        if (IsOnGround == true)
        {
            gravity = 0;
            
        }

        if (IsOnGround == false)
        {
            gravity = gravityForce;
        }

    
        
        // setting the moemevnts for animations to be reletive to the camera
        if(cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1,0,1).normalized);
            move = mV * camForward + mH * cam.right;
        }
        else
        {
            move = mV * transform.forward + mH * transform.right;
        }
        if(move.magnitude > 1)
        {
            move.Normalize();
        }
        // rotate towards the mouse
        Rotate();
        // Check for ground (Main for animations, but also for jump limits)
        
        
        Move(move);
        

        Vector3 movement = new Vector3(mH , gravity, mV * 2.2f);
        movement = CameraOrient.transform.TransformDirection(movement);
        rb.velocity = movement * speed;

    }

    // so far not working
    void JumpPressed()
    {
        if (Input.GetButtonDown("Jump") && IsOnGround == true)
        {
            Debug.Log("Jumped");
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            
            
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
        animator.SetFloat("Forward", forwardAmount * 5);
        animator.SetFloat("Turn", turnAmount);
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

        if (Physics.Raycast(_gRay, out _gHit, 1.5f))
        {
            IsOnGround = true;
        }
        else
        {
            IsOnGround = false;
        }

        Debug.DrawRay(transform.position, -transform.up * 1.6f, Color.white);
    }

    
    
}
