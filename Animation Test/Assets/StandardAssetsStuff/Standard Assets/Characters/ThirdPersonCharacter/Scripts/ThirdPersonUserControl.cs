using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


        // added by JACKSON LANAUS
        public Transform spineBone;
        private Vector3 mousePos;
        public float LookAngle;
        Animator m_Animator;
        

        public bool ConstrainedMove = true;
        

        private void Start()
        {

            m_Animator = GetComponent<Animator>();
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;

        }

        private void LateUpdate()
        {
            // ADDED BY JACKSON LANAUS

            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {

                mousePos = _hit.point;

            }
            
            if(ConstrainedMove == true)
            {
                if (Vector3.Angle(transform.forward, mousePos - transform.position) < LookAngle) //subtracting the transform position from the mousePos got the angle calculation I was looking for
                {

                    Vector3 targetPos = new Vector3(mousePos.x, spineBone.transform.position.y, mousePos.z); //this limits the LookAt to only rotate on the X axis by basically saying keep the y axis between the spineBone and the mousePos the same
                    spineBone.LookAt(targetPos);


                }

                else
                {

                    //rotate the whole character's body
                }
            }

            if (ConstrainedMove == false)
            {
                Vector3 targetPos = new Vector3(mousePos.x, spineBone.transform.position.y, mousePos.z); //this limits the LookAt to only rotate on the X axis by basically saying keep the y axis between the spineBone and the mousePos the same
                spineBone.LookAt(targetPos);
            }

            //Debug.Log(Vector3.Angle(transform.forward, mousePos - transform.position));

            

            

            //NOT ADDED BY JACKOSN LANAUS, STARTING HERE.
        }
    }
}
