using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{

    public Transform spineBone;
    private Vector3 mousePos;
    public float LookAngle;

    Animator m_Animator;
    bool IsBackward = false;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {

            mousePos = _hit.point;

        }
        Vector3 targetPos = new Vector3(mousePos.x, spineBone.transform.position.y, mousePos.z); //this limits the LookAt to only rotate on the X axis by basically saying keep the y axis between the spineBone and the mousePos the same
        spineBone.LookAt(targetPos);
        //Debug.Log(Vector3.Angle(transform.forward, mousePos - transform.position));

        if (Vector3.Angle(transform.forward, mousePos - transform.position) < LookAngle) //subtracting the transform position from the mousePos got the angle calculation I was looking for
        {
            
            IsBackward = false;
        }

        else
        {
            IsBackward = true;
            //rotate the whole character's body
        }

        m_Animator.SetBool("Backward", IsBackward);
    }
}

