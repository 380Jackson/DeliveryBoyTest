using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Camera cam;
    public Animator animator;
    public float mH;
    public float mV;
    public float speed;
    private Rigidbody rb;
    
    float angle;

    void Start()
    {
        //Animator animator = GetComponent<Animator>();
        
        rb = GetComponent<Rigidbody>();
        
    }
    private void Update()
    {
        Rotate();
        Animate();
    }
    void FixedUpdate()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(mH * speed, rb.velocity.y, mV * speed);  
    }
    void Animate()
    {
        /* START LOCOMOTION */
        // Update the Animator with our values so that the blend tree updates
        animator.SetFloat("VelX", mH/2);
        animator.SetFloat("VelZ", mV/2);
        /* END LOCOMOTION */

    }
    void Rotate()
    {

        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray ,out _hit))
        {
            transform.LookAt(new Vector3(_hit.point.x, transform.position.y, _hit.point.z));

            
        }

    }

    
}
